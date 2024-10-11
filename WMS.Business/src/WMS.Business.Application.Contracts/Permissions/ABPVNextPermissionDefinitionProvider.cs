using WMS.Business.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace WMS.Business.Permissions;

public class ABPVNextPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ABPVNextPermissions.GroupName, L("Permission:ABPVNext"));

        var dictionary = myGroup.AddPermission(ABPVNextPermissions.DataDictionary.Default, L("DataDictionary"));
        dictionary.AddChild(ABPVNextPermissions.DataDictionary.Update, L("Edit"));
        dictionary.AddChild(ABPVNextPermissions.DataDictionary.Delete, L("Delete"));
        dictionary.AddChild(ABPVNextPermissions.DataDictionary.Create, L("Create"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ABPVNextResource>(name);
    }
}
