using Com.Filter.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace WMS.BaseService.BaseEntity
{
    /// <summary>
    /// 多租户聚合根基类
    /// </summary>
    public class FullAuditedAggregateRootBase : FullAuditedAggregateRoot<Guid>, IMultiTenant, ICompany
    {
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
        /// <summary>
        /// 公司ID赋值
        /// </summary>
        /// <param name="companyid"></param>
        public void SetCompanyId(string companyid)
        {
            bool isValid = Guid.TryParse(companyid, out Guid companyId);
            if (isValid)
            {
                this.CompanyId = companyId;
            }
            else
            {
                this.CompanyId = null;
            }
        }
    }
}
