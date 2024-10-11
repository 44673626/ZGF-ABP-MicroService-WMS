using HangFireJob.Dispatcher;
using HangFireJob.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace HangFireJob.Samples.BackgroundJob
{
    public class NotifyJob : BackgroundJob<NotifyTaskArgs>, ITransientDependency
    {
        private readonly NotifyJobDispatcher _service;

        public NotifyJob(NotifyJobDispatcher service)
        {
            _service = service;


        }
        public override void Execute(NotifyTaskArgs args)
        {

            if (args != null)
            {
                _service.NotifyJob(args);
            }


        }


    }

}
