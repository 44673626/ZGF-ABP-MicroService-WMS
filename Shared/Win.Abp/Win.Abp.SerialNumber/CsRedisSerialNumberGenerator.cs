using System;
using System.Threading.Tasks;
using CSRedis;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Win.Abp.SerialNumber
{

    public class CsRedisSerialNumberGenerator : ISerialNumberGenerator
    {
        private readonly CSRedisClient _csRedis;

        private readonly string _redisConnectionString;
        private readonly string _prefix = string.Empty;
        private readonly string _dateTimeFormat = "yyyyMMdd";
        private readonly int _numberCount = 6;
        private readonly int _step = 1;
        private readonly string _separator=string.Empty;
        public AbpSerialNumberGeneratorOptions Options { get; }

        protected CsRedisSerialNumberGenerator() { }

        public CsRedisSerialNumberGenerator(IOptions<AbpSerialNumberGeneratorOptions> options)
        {
            Options = options.Value;
            this._prefix = options.Value.GetDefaultPrefix();
            this._dateTimeFormat = options.Value.GetDefaultDateTimeFormat();
            this._separator = options.Value.GetDefaultSeparator();
            this._numberCount = options.Value.GetDefaultNumberCount();
            this._step = options.Value.GetDefaultStep();
            _redisConnectionString = options.Value.RedisConnectionString;
            _csRedis = new CSRedisClient(_redisConnectionString);
            RedisHelper.Initialization(_csRedis);

        }

        public async Task<string> InitAsync(DateTime time,  string prefix = null)
        {
            return await SetAsync(time,prefix);
        }

        public async Task<string> CreateAsync(DateTime time, string datetimeFormat = null, string prefix = null,
            string separator = null, int numberCount = 0, int step = 0)
        {
            if (prefix == null) prefix = _prefix;
            if (separator == null) separator = _separator;
            if (datetimeFormat == null) datetimeFormat = _dateTimeFormat;
            if (numberCount == 0) numberCount = _numberCount;
            if (step == 0) step = _step;
            
            var serial = await GetLastSerialAsync(prefix, time, step);

            var serialNumberString= $"{prefix}{separator}" +
                   $"{time.ToString(datetimeFormat)}{separator}" +
                   $"{serial.ToString().PadLeft(numberCount, '0')}" ;
            return serialNumberString;
        }

        public async Task<string> SetAsync(DateTime time,  string prefix = null, int serial = 0)
        {
            var key = GetSerialNumberKey(prefix, time);
            await _csRedis.SetAsync(key, serial);
            return await _csRedis.GetAsync(key);
        }


        private async Task<long> GetLastSerialAsync(string prefix, DateTime time, int step)
        {
            var key = GetSerialNumberKey(prefix, time);
            var serial = await _csRedis.IncrByAsync(key, step);
            Console.WriteLine($"CsRedis:{key} {serial}");
            return serial;
        }

        private static string GetSerialNumberKey(string prefix, DateTime time)
        {
            var key = $"{prefix}:{time:yyyyMMdd}";
            return key;
        }
    }
}
