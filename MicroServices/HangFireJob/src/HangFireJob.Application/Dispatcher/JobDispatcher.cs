using HangFireJob.EventArgs;
using HangFireJob.Samples.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;

namespace HangFireJob.Dispatcher
{
    public class ImportJobDispatcher : ITransientDependency
    {

        private IImportJob _importService;

        private readonly Func<string, IImportJob> _importServiceAccessor;

        public ImportJobDispatcher(
           Func<string, IImportJob> importServiceAccessor
           )
        {
            _importServiceAccessor = importServiceAccessor;
        }
        /// <summary>
        /// 分配导入给对应服务
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public Task ImportJob(ImportTaskArgs args)
        {
            _importService = _importServiceAccessor(args.ServiceName);
            _importService.ImportFile(args.Id, args.FileName, args.FileName, args.InputConditions);
            return Task.CompletedTask;
        }

    }


    public class ExportJobDispatcher : ITransientDependency
    {
        private IExportJob _exportService;
        private readonly Func<string, IExportJob> _exportServiceAccessor;
        public ExportJobDispatcher(
           Func<string, IExportJob> exportServiceAccessor
           )
        {
            _exportServiceAccessor = exportServiceAccessor;
        }
        public async Task ExportJob(ExportTaskArgs args)
        {
            _exportService = _exportServiceAccessor(args.ServiceName);
            //var _exportService = (IExportJob)_serviceProvider.GetService(Type.GetType(args.ServiceName));
            await _exportService.ExportFile(args.Id, args.DownFileName, args.InputConditions);
            // return Task.CompletedTask;
        }


    }

    public class NotifyJobDispatcher : ITransientDependency
    {

        private INotifyJob _notifyService;
        private readonly Func<string, INotifyJob> _notifyServiceAccessor;
        private readonly IEmailSender _emailSender;
        public NotifyJobDispatcher(
           Func<string, INotifyJob> notifyServiceAccessor,
           IEmailSender emailSender

           )
        {
            _notifyServiceAccessor = notifyServiceAccessor;
            _emailSender = emailSender;
        }



        public Task NotifyJob(NotifyTaskArgs args)
        {
            //_emailSender.SendAsync()
            _notifyService = _notifyServiceAccessor(args.ServiceName);
            ////var _notifyService = (INotifyJob)_serviceProvider.GetService(Type.GetType(args.ServiceName));
            _notifyService.SendNotify(args.Id, args.FileName, args.RealFileName, args.InputConditions);
            return Task.CompletedTask;

        }
    }
}
