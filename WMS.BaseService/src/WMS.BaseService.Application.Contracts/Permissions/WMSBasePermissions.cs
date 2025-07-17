using Volo.Abp.Reflection;

namespace WMS.BaseService.Permissions;

public class WMSBasePermissions
{
    public const string GroupName = "WMSBase";

    public static class DataDictionary
    {
        public const string Default = GroupName + ".DataDictionary";
        public const string Delete = Default + ".Delete";
        public const string Update = Default + ".Update";
        public const string Create = Default + ".Create";
    }

    public static class DataDictionaryDetail
    {
        public const string Default = GroupName + ".DataDictionaryDetail";
        public const string Delete = Default + ".Delete";
        public const string Update = Default + ".Update";
        public const string Create = Default + ".Create";
    }

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(WMSBasePermissions));
    }
}
