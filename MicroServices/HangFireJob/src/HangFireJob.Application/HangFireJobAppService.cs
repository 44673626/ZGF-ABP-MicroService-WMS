using HangFireJob.Localization;
using Volo.Abp.Application.Services;

namespace HangFireJob;

public abstract class HangFireJobAppService : ApplicationService
{
    protected HangFireJobAppService()
    {
        LocalizationResource = typeof(HangFireJobResource);
        ObjectMapperContext = typeof(HangFireJobApplicationModule);
    }
}
