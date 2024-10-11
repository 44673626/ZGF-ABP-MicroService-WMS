using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Reflection;

namespace FileStorage.Permissions
{
    public static class FileStoragePermissions
    {
        public const string GroupName = "FileStorage";

        public const string CreateStr = "Create";
        public const string UpdateStr = "Update";
        public const string DeleteStr = "Delete";

        public static class UploadFile
        {
            //public const string Default = GroupName + "." + nameof(UploadFile);
            public const string Default = GroupName + ".UploadFile";
            public const string Create = Default + "." + CreateStr;
            public const string Update = Default + "." + UpdateStr;
            public const string Delete = Default + "." + DeleteStr;
        }
        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(FileStoragePermissions));
        }
    }
}
