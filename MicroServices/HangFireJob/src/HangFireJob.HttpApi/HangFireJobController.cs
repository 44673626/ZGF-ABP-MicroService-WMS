using HangFireJob.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace HangFireJob;

public abstract class HangFireJobController : AbpControllerBase
{
    protected HangFireJobController()
    {
        LocalizationResource = typeof(HangFireJobResource);
    }
}
