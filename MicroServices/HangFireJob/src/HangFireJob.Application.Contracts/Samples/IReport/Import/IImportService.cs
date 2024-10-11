using HangFireJob.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace HangFireJob.Samples.IReport.Import
{

    public interface IImportService : ITransientDependency
    {
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <returns></returns>
        Task<string> AddBackGroundJob(Guid id, List<string> exportName, List<string> realfileName, List<CustomCondition> p_list);

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        Task DeleteJob(string jobId);

        /// <summary>
        /// 重新进入队列
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        Task<bool> Requeue(string jobId);
    }
}
