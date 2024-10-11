using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace ABP.Business.EntityFrameworkCore;

[ConnectionStringName(ABPVNextDbProperties.ConnectionStringName)]
public interface IABPVNextDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
