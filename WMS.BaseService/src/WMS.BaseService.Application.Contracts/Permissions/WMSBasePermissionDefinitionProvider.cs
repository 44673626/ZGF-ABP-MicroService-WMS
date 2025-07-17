using WMS.BaseService.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace WMS.BaseService.Permissions;

public class WMSBasePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(WMSBasePermissions.GroupName, L("Permission:ABPVNext"));

        var dictionary = myGroup.AddPermission(WMSBasePermissions.DataDictionary.Default, L("DataDictionary"));
        dictionary.AddChild(WMSBasePermissions.DataDictionary.Update, L("Edit"));
        dictionary.AddChild(WMSBasePermissions.DataDictionary.Delete, L("Delete"));
        dictionary.AddChild(WMSBasePermissions.DataDictionary.Create, L("Create"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<WMSBaseResource>(name);
    }
}
