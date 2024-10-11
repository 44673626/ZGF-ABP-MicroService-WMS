using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Volo.Abp;

namespace Win.Abp.SerialNumber.Test
{
    public class SerialNumberTestsHostedService : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var application = AbpApplicationFactory.Create<SerialNumberTestsModule>(options =>
            {
                options.Services.AddLogging(loggingBuilder => { });
            }))
            {
                application.Initialize();

                var serialNumberTestService = application.ServiceProvider.GetRequiredService<SerialNumberTestService>();
                await serialNumberTestService.RunAsync();

                application.Shutdown();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}