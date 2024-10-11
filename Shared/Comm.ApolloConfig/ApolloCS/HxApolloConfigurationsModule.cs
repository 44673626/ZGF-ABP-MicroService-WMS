using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace Com.ApolloConfig.ApolloCS
{
    public class HxApolloConfigurationsModule : AbpModule
    {
        //public override void ConfigureServices(ServiceConfigurationContext context)
        //{
        //    context.Services.AddTransient<MesConfigurations>();
        //}
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var apolloConfigurationReader = context.ServiceProvider.GetRequiredService<MesConfigurations>();
            apolloConfigurationReader.UseApollo();//启动apollo配置
        }

    }

}
