namespace ABP.Business;

public static class ABPVNextDbProperties
{
    public static string DbTablePrefix { get; set; } = "ABPVNext";

    public static string DbSchema { get; set; } = null;

    public const string ConnectionStringName = "Business";
}
