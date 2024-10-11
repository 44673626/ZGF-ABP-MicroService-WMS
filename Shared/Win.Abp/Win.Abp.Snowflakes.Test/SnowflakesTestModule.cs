using Volo.Abp.Modularity;
using Win.Abp.Snowflakes;

namespace Win.Abp.Snowflakes.Test
{
    [DependsOn(
        typeof(AbpSnowflakeGeneratorModule)
    )]
    public class SnowflakesTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpSnowflakeIdGeneratorOptions>(options =>
            {
                options.DefaultDatacenterId = 2;
                options.DefaultWorkerId = 4;
            });
        }
    }
}