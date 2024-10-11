using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Volo.Abp;

namespace Win.Abp.Snowflakes.Test
{
    public class SnowflakesTestHostedService : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var application = AbpApplicationFactory.Create<SnowflakesTestModule>(options =>
            {
                options.Services.AddLogging(loggingBuilder => { });
            }))
            {
                application.Initialize();

                var demo = application.ServiceProvider.GetRequiredService<SnowflakesTestService>();
                await demo.RunAsync();

                application.Shutdown();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}