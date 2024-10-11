namespace HangFireJob;

public static class HangFireJobDbProperties
{
    public static string DbTablePrefix { get; set; } = "HangFireJob";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "HangFireJob";
}
