using Volo.Abp.Reflection;

namespace WMS.Business.Permissions;

public class ABPVNextPermissions
{
    public const string GroupName = "ABPVNext";

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
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(ABPVNextPermissions));
    }
}
