using HangFireJob.EventArgs;
using HangFireJob.Samples.ExportJob;
using HangFireJob.Samples.IReport.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;

namespace HangFireJob.Samples.Report.Import
{
    /// <summary>
    /// 导入大数据文件
    /// </summary>
    public class HImportService : ITransientDependency, IImportService
    {
        private readonly HImportJob _job_hq;

        public HImportService(
            HImportJob job_hq
            )
        {
            _job_hq = job_hq;
        }
        public async Task<string> AddBackGroundJob(Guid id, List<string> exportName, List<string> realfileName, List<CustomCondition> p_list)
        {
            //任务ID
            var jobId = string.Empty;
            //创建异步任务
            //Hangfire 注册的时候默认是单例模式，所以在任意代码中使用其静态方法就能添加异步任务或者定时任务
            jobId = Hangfire.BackgroundJob.Schedule(() => _job_hq.ImportFile(id, exportName, realfileName, p_list), TimeSpan.FromSeconds(10));
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
