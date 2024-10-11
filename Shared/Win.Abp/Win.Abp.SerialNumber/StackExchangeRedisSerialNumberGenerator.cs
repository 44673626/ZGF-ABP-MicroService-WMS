using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Win.Abp.SerialNumber
{
    public class StackExchangeRedisSerialNumberGenerator:ISerialNumberGenerator
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _db;
        private readonly string _prefix = string.Empty;
        private readonly string _dateTimeFormat = "yyyyMMdd";
        private readonly int _numberCount = 6;
        private readonly int _step = 1;
        private readonly string _separator = string.Empty;
        public AbpSerialNumberGeneratorOptions Options { get; }

        protected StackExchangeRedisSerialNumberGenerator() { }

        public StackExchangeRedisSerialNumberGenerator(IOptions<AbpSerialNumberGeneratorOptions> options)
        {
            Options = options.Value;
            this._prefix = options.Value.GetDefaultPrefix();
            this._dateTimeFormat = options.Value.GetDefaultDateTimeFormat();
            this._separator = options.Value.GetDefaultSeparator();
            this._numberCount = options.Value.GetDefaultNumberCount();
            this._step = options.Value.GetDefaultStep();
            var redisConnectionString = options.Value.RedisConnectionString;

            _redis = ConnectionMultiplexer.Connect(redisConnectionString);
            _db = _redis.GetDatabase();

        }

        public async Task<string> InitAsync(DateTime time, string prefix = null)
        {
            return await SetAsync(time, prefix);
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

            var serialNumberString = $"{prefix}{separator}" +
                   $"{time.ToString(datetimeFormat)}{separator}" +
                   $"{serial.ToString().PadLeft(numberCount, '0')}";
            return serialNumberString;
        }

        public async Task<string> SetAsync(DateTime time, string prefix = null, int serial = 0)
        {
            var key = GetSerialNumberKey(prefix, time);
            await _db.StringSetAsync(key, serial);
            return await _db.StringGetAsync(key);
        }


        private async Task<long> GetLastSerialAsync(string prefix, DateTime time, int step)
        {
            var key = GetSerialNumberKey(prefix, time);
            var serial = await _db.StringIncrementAsync(prefix, step);
            Console.WriteLine($"StackExchangeRedis:{key} {serial}");

            return serial;
        }

        private static string GetSerialNumberKey(string prefix, DateTime time)
        {
            var key = $"{prefix}:{time:yyyyMMdd}";
            return key;
        }
    }
}