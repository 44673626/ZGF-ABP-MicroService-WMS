using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace WMS.BaseService.EntityFrameworkCore;

[ConnectionStringName(WMSBaseDbProperties.ConnectionStringName)]
public interface IWMSBaseDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
