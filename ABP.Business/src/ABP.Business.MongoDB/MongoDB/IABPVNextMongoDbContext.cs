using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace ABP.Business.MongoDB;

[ConnectionStringName(ABPVNextDbProperties.ConnectionStringName)]
public interface IABPVNextMongoDbContext : IAbpMongoDbContext
{
    /* Define mongo collections here. Example:
     * IMongoCollection<Question> Questions { get; }
     */
}
