using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Win.Abp.SerialNumber;

namespace Win.Abp.SerialNumber.Test
{
    [DependsOn(
        typeof(AbpSerialNumberGeneratorModule)
    )]
    public class SerialNumberTestsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

        }
    }
}