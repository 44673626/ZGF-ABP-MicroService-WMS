using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Core;
using Volo.Abp.DependencyInjection;
using Win.Abp.Snowflakes;

namespace Win.Abp.Snowflakes.Test
{
    public class SnowflakesTestService : ITransientDependency
    {
        private ISnowflakeIdGenerator _snowflakeIdGenerator;
        private AbpSnowflakeIdGeneratorOptions _options;

        public SnowflakesTestService(
            ISnowflakeIdGenerator snowflakeIdGenerator,
            IOptions<AbpSnowflakeIdGeneratorOptions> options
        )
        {
            _snowflakeIdGenerator = snowflakeIdGenerator;
            _options = options.Value;
        }

        public async Task RunAsync()
        {
            Log.Information("SnowflakeId Generator Test");
            await TestSnowflakeIdGenerator();
        }

        private async Task TestSnowflakeIdGenerator()
        {
            for (var i = 0; i < 10; i++)
            {
                var id = _snowflakeIdGenerator.Create();
                var info = $"{_options.DefaultDatacenterId} {_options.DefaultWorkerId} {DateTime.Now} {id}";
                Log.Information(info);
                await Task.Delay(100);
            }
        }
    }
}