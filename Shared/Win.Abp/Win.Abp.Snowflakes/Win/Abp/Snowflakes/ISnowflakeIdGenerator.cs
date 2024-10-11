namespace Win.Abp.Snowflakes
{
    public interface ISnowflakeIdGenerator
    {
        /// <summary>
        /// Creates a new Snowflake Id.
        /// </summary>
        /// <returns></returns>
        long Create();

        string AnalyzeId(long id);

        // long Create(long datacenterId, long workerId);
    }
}