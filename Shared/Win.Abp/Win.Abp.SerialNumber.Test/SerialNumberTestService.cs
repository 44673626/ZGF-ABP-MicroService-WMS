using System;
using System.Diagnostics.Eventing.Reader;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Core;
using Volo.Abp.DependencyInjection;
using Win.Abp.SerialNumber;

namespace Win.Abp.SerialNumber.Test
{
    public class SerialNumberTestService : ITransientDependency
    {
        private ISerialNumberGenerator _serialNumberGenerator;
        private AbpSerialNumberGeneratorOptions _options;

        public SerialNumberTestService(
            ISerialNumberGenerator serialNumberGenerator,
            IOptions<AbpSerialNumberGeneratorOptions> options
        )
        {
            _serialNumberGenerator = serialNumberGenerator;
            _options = options.Value;
        }

        public async Task RunAsync()
        {
            Log.Information("Serial Number Generator Test");
            await TestSerialNumberGenerator();
        }

        private async Task TestSerialNumberGenerator()
        {
            string id;
            var date = DateTime.Today;
            var datetimeFormat = "yyyyMMdd";
            var prefix = "TEST";
            var separator = "-";
            var numberCount = 6;
            var step = 2;

            var count = 15;
            Log.Information($"Generator count: {count}");
            for (int i = 0; i < count; i++)
            {
                id = await _serialNumberGenerator.CreateAsync(date,datetimeFormat: datetimeFormat, prefix: prefix, separator: separator, numberCount: numberCount, step: step);
                Log.Information(id);
                await Task.Delay(100);
            }

            Log.Information("Init Serial Number : 0");
            id = await _serialNumberGenerator.InitAsync(date, prefix);
            Log.Information(id);

            var setToValue = 100;
            Log.Information($"Set Serial Number : {setToValue}");
            id = await _serialNumberGenerator.SetAsync(date, prefix, setToValue);
            Log.Information(id);

            Log.Information($"Generator count: {count}");
            for (int i = 0; i < count; i++)
            {
                id = await _serialNumberGenerator.CreateAsync(date, datetimeFormat: datetimeFormat, prefix: prefix, separator: separator, numberCount: numberCount, step: step);
                Log.Information(id);
                await Task.Delay(100);
            }

        }
    }
}