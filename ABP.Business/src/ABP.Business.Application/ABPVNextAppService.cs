using ABP.Business.Localization;
using Volo.Abp.Application.Services;

namespace ABP.Business;

public abstract class ABPVNextAppService : ApplicationService
{
    protected ABPVNextAppService()
    {
        LocalizationResource = typeof(ABPVNextResource);
        ObjectMapperContext = typeof(ABPVNextApplicationModule);
    }
}
