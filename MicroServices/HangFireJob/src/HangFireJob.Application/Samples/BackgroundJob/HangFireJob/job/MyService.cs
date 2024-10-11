using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.BackgroundJobs;

namespace HangFireJob.Samples.BackgroundJob.HangFireJob.job
{
    public class MyService : ApplicationService
    {
        private readonly IBackgroundJobManager _backgroundJobManager;

        public MyService(IBackgroundJobManager backgroundJobManager)
        {
            _backgroundJobManager = backgroundJobManager;
        }

        public async Task Test()
        {
            await _backgroundJobManager.EnqueueAsync(new MyJobArgs("42"));
        }
    }
}
