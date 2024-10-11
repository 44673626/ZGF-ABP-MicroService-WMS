using Volo.Abp.Authorization.Permissions;
using Win.Sfs.FileStorage.Localization;

using Volo.Abp.Localization;


namespace FileStorage.Permissions
{
    public class FileStoragePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(FileStoragePermissions.GroupName, L($"Permission:{FileStoragePermissions.GroupName}"));

            var boms = myGroup.AddPermission(FileStoragePermissions.UploadFile.Default, L("StorageFile"));
            boms.AddChild(FileStoragePermissions.UploadFile.Create, L("Create"));
            boms.AddChild(FileStoragePermissions.UploadFile.Update, L("Update"));
            boms.AddChild(FileStoragePermissions.UploadFile.Delete, L("Delete"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<FileStorageResource>(name);
        }

    }
}
