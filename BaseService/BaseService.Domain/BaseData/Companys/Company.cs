using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using Volo.Abp;

namespace BaseService.BaseData.Companys
{
    /// <summary>
    /// 公司基本信息
    /// </summary>
    public class Company : AuditedAggregateRoot<Guid>, ISoftDelete, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public bool IsDeleted { get; set; }
        /// <summary>
        /// 公司编码
        /// </summary>
        public string CompanyCode { get; set; }
        /// <summary>
        /// 公司名称 
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>
        public Guid? Pid { get; set; }
        /// <summary>
        /// 是否是总公司
        /// </summary>
        public bool WhethertoHeadOffice { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 启用日期
        /// </summary>
        public DateTime RestartDate { get; set; } = DateTime.Now;
        /// <summary>
        /// 启用人ID
        /// </summary>
        public Guid RestartPersonId { get; set; }
        /// <summary>
        /// 启用原因
        /// </summary>
        public string RestartReason { get; set; }
        /// <summary>
        /// 停用日期
        /// </summary>
        public DateTime? DeactivationDate { get; set; }
        /// <summary>
        /// 停用人ID
        /// </summary>
        public Guid? DeactivatePersonId { get; set; }
        /// <summary>
        /// 停用原因
        /// </summary>
        public string DeactivationReason { get; set; }
        /// <summary>
        /// 删除原因
        /// </summary>
        public string DeletionReason { get; set; }
        /// <summary>
        /// ID赋值
        /// </summary>
        /// <param name="id"></param>
        public void SetId(Guid id)
        {
            this.Id = id;
        }

        public Company(Guid? tenantId, Guid? pid, Guid id, string companyName, bool enabled,
           bool whethertoHeadOffice, Guid restartPersonId, string restartReason)
        {
            Id = id;
            TenantId = tenantId;
            Pid = pid;
            CompanyName = companyName;
            Enabled = enabled;
            WhethertoHeadOffice = whethertoHeadOffice;
            RestartPersonId = restartPersonId;
            RestartReason = restartReason;
        }

    }
}
