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
    public class ExportJob : BackgroundJob<ExportTaskArgs>, ITransientDependency
    {
        private readonly ExportJobDispatcher _service;

        public ExportJob(ExportJobDispatcher service)
        {
            _service = service;


        }
        public override void Execute(ExportTaskArgs args)
        {
            if (args != null)
            {

                _service.ExportJob(args);
            }

        }


    }

}
