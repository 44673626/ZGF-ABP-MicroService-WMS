using Comm.ConsulConfig.Registry;
using Comm.ConsulConfig.Registry.ConsulService;
using Comm.ConsulConfig.Registry.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comm.ConsulConfig.Registry.Extentions
{
    /// <summary>
    ///  服务注册IOC容器扩展
    /// </summary>
    public static class ServiceRegistryServiceCollectionExtensions
    {
        // consul服务注册
        public static IServiceCollection AddServiceRegistry(this IServiceCollection services)
        {
            services.AddServiceRegistry(optons => { });
            return services;
        }

        // consul服务注册
        public static IServiceCollection AddServiceRegistry(this IServiceCollection services, Action<ServiceRegistryOptions> options)
        {
            // 1、配置选项到IOC
            services.Configure(options);

            // 2、注册consul注册
            services.AddSingleton<IServiceRegistry, ConsulServiceRegistry>();

            // 3、注册开机自动注册服务
            services.AddHostedService<ServiceRegistryIHostedService>();

            return services;
        }
    }
}
