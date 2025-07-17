namespace WMS.BaseService;

public static class WMSBaseDbProperties
{
    public static string DbTablePrefix { get; set; } = "WMSBase";

    public static string DbSchema { get; set; } = null;

    public const string ConnectionStringName = "Business";
}
