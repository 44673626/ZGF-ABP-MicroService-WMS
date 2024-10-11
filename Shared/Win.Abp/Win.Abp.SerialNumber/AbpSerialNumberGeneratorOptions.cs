using System.Dynamic;

namespace Win.Abp.SerialNumber
{
    public class AbpSerialNumberGeneratorOptions
    {
        public string RedisConnectionString { get; set; }

        public string Prefix { get; set; }

        public string Postfix { get; set; }

        public string DateTimeFormat { get; set; }

        public string Separator { get; set; }

        public int? NumberCount { get; set; }
        
        public int? Step { get; set; }
        public string GetDefaultPrefix()
        {
            return Prefix ?? string.Empty;
        }

        public string GetDefaultDateTimeFormat()
        {
            return DateTimeFormat ?? "yyyyMMdd";
        }

        public int GetDefaultNumberCount()
        {
            return NumberCount ?? 6;
        }

        public string GetDefaultRedisConnectionString()
        {
            return RedisConnectionString?? "127.0.0.1";
        }

        public int GetDefaultStep()
        {
            return Step??1;
        }

        public string GetDefaultPostfix()
        {
            return Postfix ?? string.Empty;
        }

        public string GetDefaultSeparator()
        {
            return Separator ?? string.Empty;

        }
    }
}