using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace WMS.BaseService;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(WMSBaseDomainSharedModule)
)]
public class WMSBaseDomainModule : AbpModule
{

}
