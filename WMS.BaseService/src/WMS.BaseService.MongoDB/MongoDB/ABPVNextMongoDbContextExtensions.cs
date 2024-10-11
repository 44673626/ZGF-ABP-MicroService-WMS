using Volo.Abp;
using Volo.Abp.MongoDB;

namespace WMS.BaseService.MongoDB;

public static class ABPVNextMongoDbContextExtensions
{
    public static void ConfigureABPVNext(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));
    }
}
