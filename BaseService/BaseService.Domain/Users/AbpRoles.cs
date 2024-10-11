using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace BaseService.Users
{
    public class AbpRoles : Entity<Guid>, IMultiTenant
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public Guid? TenantId { get; set; }
        public string ConcurrencyStamp { get; set; }
    }
}
