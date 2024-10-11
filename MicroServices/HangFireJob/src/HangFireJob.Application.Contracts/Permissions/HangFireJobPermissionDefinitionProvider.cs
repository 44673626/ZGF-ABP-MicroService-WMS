using HangFireJob.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace HangFireJob.Permissions;

public class HangFireJobPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(HangFireJobPermissions.GroupName, L("Permission:HangFireJob"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<HangFireJobResource>(name);
    }
}
