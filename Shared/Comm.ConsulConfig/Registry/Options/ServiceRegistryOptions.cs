using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comm.ConsulConfig.Registry.Options
{
    /// <summary>
    /// 节点注册选项
    /// </summary>
    public class ServiceRegistryOptions
    {
        public ServiceRegistryOptions()
        {
            ServiceId = Guid.NewGuid().ToString();
            RegistryAddress = "http://localhost:8500";
            HealthCheckAddress = "/HealthCheck";
        }

        /// <summary>
        /// 服务ID
        /// </summary>
        public string ServiceId { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 服务地址http://localhost:5001
        /// </summary>
        public string ServiceAddress { get; set; }

        /// <summary>
        /// 服务标签(版本)
        /// </summary>
        public string[] ServiceTags { set; get; }

        /*// 服务地址(可以选填 === 默认加载启动路径(localhost))
        public string ServiceAddress { set; get; }

        // 服务端口号(可以选填 === 默认加载启动路径端口)
        public int ServicePort { set; get; }

        // Https 或者 http
        public string ServiceScheme { get; set; }*/

        /// <summary>
        /// 服务注册地址
        /// </summary>
        public string RegistryAddress { get; set; }

        /// <summary>
        /// 服务健康检查地址
        /// </summary>
        public string HealthCheckAddress { get; set; }
        /// <summary>
        /// 服务描述
        /// </summary>
        public string ServiceDescription { get; set; }
    }
}
