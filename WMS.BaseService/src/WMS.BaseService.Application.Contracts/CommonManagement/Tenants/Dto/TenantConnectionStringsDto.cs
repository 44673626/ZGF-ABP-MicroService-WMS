using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.BaseService.CommonManagement.Tenants.Dto
{
    /// <summary>
    /// 创建租户独立数据库DTO
    /// </summary>
    public class TenantConnectionStringsDto
    {
        /// <summary>
        /// 租户ID
        /// </summary>
        public Guid? TenantId { get; set; }
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 租户数据库连接字符串
        /// </summary>
        public string TenantConnection { get; set; }
        /// <summary>
        /// 扩展字段
        /// </summary>
        public string TenantConnDataBaseName { get; set; }
    }
}
