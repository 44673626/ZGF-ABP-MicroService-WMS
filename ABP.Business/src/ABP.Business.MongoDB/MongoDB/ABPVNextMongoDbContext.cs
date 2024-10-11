using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace ABP.Business.MongoDB;

[ConnectionStringName(ABPVNextDbProperties.ConnectionStringName)]
public class ABPVNextMongoDbContext : AbpMongoDbContext, IABPVNextMongoDbContext
{
    /* Add mongo collections here. Example:
     * public IMongoCollection<Question> Questions => Collection<Question>();
     */

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigureABPVNext();
    }
}
