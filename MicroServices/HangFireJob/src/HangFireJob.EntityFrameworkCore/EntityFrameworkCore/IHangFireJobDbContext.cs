using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace HangFireJob.EntityFrameworkCore;

[ConnectionStringName(HangFireJobDbProperties.ConnectionStringName)]
public interface IHangFireJobDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
