using ABP.Business.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace ABP.Business;

public abstract class ABPVNextController : AbpControllerBase
{
    protected ABPVNextController()
    {
        LocalizationResource = typeof(ABPVNextResource);
    }
}
