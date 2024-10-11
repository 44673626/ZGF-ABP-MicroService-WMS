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
    public class ImportJob : BackgroundJob<ImportTaskArgs>, ITransientDependency
    {
        private readonly ImportJobDispatcher _service;

        public ImportJob(ImportJobDispatcher service)
        {
            _service = service;
        }
        public override void Execute(ImportTaskArgs args)
        {
            if (args != null)
            {
                _service.ImportJob(args);
            }

        }
    }
}
