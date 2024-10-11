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
    ///  服务发现IOC容器扩展
    /// </summary>
    public static class ServiceDiscoveryServiceCollectionExtensions
    {
        // consul服务发现
        public static IServiceCollection AddServiceDiscovery(this IServiceCollection services)
        {
            // 1、注册consul服务发现
            services.AddServiceDiscovery(options => { });
            return services;
        }

        public static IServiceCollection AddServiceDiscovery(this IServiceCollection services,
                                                      Action<ServiceDiscoveryOptions> options)
        {
            // 2、注册到IOC容器
            services.Configure(options);

            // 3、注册consul服务发现
            services.AddSingleton<IServiceDiscovery, ConsulServiceDiscovery>();

            return services;
        }
    }
}
