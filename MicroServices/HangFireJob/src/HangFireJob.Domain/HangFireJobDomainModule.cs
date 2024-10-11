using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace HangFireJob;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(HangFireJobDomainSharedModule)
)]
public class HangFireJobDomainModule : AbpModule
{

}
