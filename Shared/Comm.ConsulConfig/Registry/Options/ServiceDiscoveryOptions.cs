using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comm.ConsulConfig.Registry.Options
{
    /// <summary>
    /// 服务发现选项
    /// </summary>
    public class ServiceDiscoveryOptions
    {
        public ServiceDiscoveryOptions()
        {
            DiscoveryAddress = "http://localhost:8500";
        }

        /// <summary>
        /// 服务发现地址
        /// </summary>
        public string DiscoveryAddress { set; get; }
    }
}
