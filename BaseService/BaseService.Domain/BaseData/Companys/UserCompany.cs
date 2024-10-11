using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace BaseService.BaseData.Companys
{
    public class UserCompany : Entity, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public Guid UserId { get; set; }

        public Guid CompanyId { get; set; }

        public override object[] GetKeys()
        {
            return new object[] { UserId, CompanyId };
        }

        public UserCompany(Guid? tenantId, Guid userId, Guid companyId)
        {
            TenantId = tenantId;
            UserId = userId;
            CompanyId = companyId;
        }
    }
}
