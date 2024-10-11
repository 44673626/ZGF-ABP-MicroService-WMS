namespace Win.Abp.Snowflakes
{
    public class AbpSnowflakeIdGeneratorOptions
    {
        // 数据中心ID(0~31) 
        public long? DefaultDatacenterId { get; set; }

        // 工作机器ID(0~31) 
        public long? DefaultWorkerId { get; set; }

        
        public long GetDefaultDatacenterId()
        {
            return DefaultDatacenterId ?? 0L;
        }

        public long GetDefaultWorkerId()
        {
            return DefaultWorkerId ?? 0L;
        }
    }
}