using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Modularity;
using Microsoft.Extensions.Hosting;


namespace Win.Abp.SerialNumber
{
    public class AbpSerialNumberGeneratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            ConfigureRedis(context, configuration);
        }

        private void ConfigureRedis(ServiceConfigurationContext context, IConfiguration configuration)
        {
            var redisConnString = configuration["Redis:Configuration"];
            Configure<AbpSerialNumberGeneratorOptions>(options => { options.RedisConnectionString = redisConnString; });
            var redisType = configuration["Redis:Type"];
            switch (redisType)
            {
                case "CsRedis":
                    context.Services.AddSingleton(typeof(ISerialNumberGenerator), typeof(CsRedisSerialNumberGenerator));
                    break;
                case "StackExchangeRedis":
                    context.Services.AddSingleton(typeof(ISerialNumberGenerator),
                        typeof(StackExchangeRedisSerialNumberGenerator));
                    break;
            }
        }
    }
}