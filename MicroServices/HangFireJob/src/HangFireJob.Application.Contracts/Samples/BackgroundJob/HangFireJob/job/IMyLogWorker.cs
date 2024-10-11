using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.BackgroundWorkers.Hangfire;

namespace HangFireJob.Samples.BackgroundJob.HangFireJob.job
{
    public interface IMyLogWorker : IHangfireBackgroundWorker
    {

    }
}
