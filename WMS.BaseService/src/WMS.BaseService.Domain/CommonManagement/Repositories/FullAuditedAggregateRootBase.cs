using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace WMS.BaseService.CommonManagement.Repositories
{
    /// <summary>
    /// 聚合根基础服务
    /// </summary>
    public class FullAuditedAggregateRootBase : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public FullAuditedAggregateRootBase() { }

        //[Newtonsoft.Json.JsonConstructor]
        [System.Text.Json.Serialization.JsonConstructor]
        public FullAuditedAggregateRootBase(Guid id) : base(id)
        {

        }
        /// <summary>
        /// 公司Id
        /// </summary>
        public Guid? CompanyId { get; set; }
        /// <summary>
        /// 租户Id
        /// </summary>
        public Guid? TenantId { get; set; }
        /// <summary>
        /// 设置Id
        /// </summary>
        /// <param name="id"></param>
        public void SetId(Guid id)
        {
            Id = id;
        }
    }
}
