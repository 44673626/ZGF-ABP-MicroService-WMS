using HangFireJob.EventArgs;
using HangFireJob.IServices.Dto;
using HangFireJob.Samples.ExportJob;
using HangFireJob.Samples.IReport.Export;
using HangFireJob.Services.BackGroudJobs;
using HangFireJob.Services.Common;
using HangFireJob.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace HangFireJob.Samples.Report.Export
{
    /// <summary>
    /// 导出大数据文件
    /// </summary>
    public class HExportService : ITransientDependency, IHExportService, IRemoteService
    {
        private readonly HExportJob _job_test;

        public HExportService(
            HExportJob job_test
            )
        {
            _job_test = job_test;

        }

        public async Task<string> AddScheduleJob(HttpJobDescriptorDto jobDescriptor)
        {
            //任务ID
            var jobId = string.Empty;
            //创建异步任务
            jobId = Hangfire.BackgroundJob.Schedule(() => HttpJobExecutor.DoRequest(jobDescriptor), TimeSpan.FromSeconds(3));
            return jobId;
        }

        public async Task<string> AddBackGroundJob(Guid id, List<string> exportName, List<CustomCondition> p_list)
        {
            //任务ID
            var jobId = string.Empty;
            //创建异步任务
            jobId = Hangfire.BackgroundJob.Schedule(() => _job_test.ExportFile(id, exportName, p_list), TimeSpan.FromSeconds(60));
            return jobId;
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="jobname"></param>
        /// <returns></returns>
        public async Task DeleteJob(string jobId)
        {
            Hangfire.BackgroundJob.Delete(jobId);
        }

        /// <summary>
        /// 重新执行任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public async Task<bool> Requeue(string jobId)
        {
            return Hangfire.BackgroundJob.Requeue(jobId);
        }
    }
}
