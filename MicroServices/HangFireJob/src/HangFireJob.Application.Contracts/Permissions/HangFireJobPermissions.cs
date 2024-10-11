using Volo.Abp.Reflection;

namespace HangFireJob.Permissions;

public class HangFireJobPermissions
{
    public const string GroupName = "HangFireJob";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(HangFireJobPermissions));
    }
}
