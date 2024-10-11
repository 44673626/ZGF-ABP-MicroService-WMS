using Comm.ConsulConfig.Registry.Extentions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Comm.ConsulConfig
{
    [DependsOn(typeof(AbpAutofacModule))]
    public class CommConsulModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            IHostEnvironment hostEnvironment = context.Services.GetSingletonInstance<IHostEnvironment>();
            context.Services.AddServiceRegistry();
        }
    }
}
