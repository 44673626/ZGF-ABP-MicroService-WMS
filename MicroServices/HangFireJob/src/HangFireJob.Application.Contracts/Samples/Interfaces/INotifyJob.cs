using HangFireJob.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.DependencyInjection;

namespace HangFireJob.Samples.Interfaces
{
    public interface INotifyJob : ITransientDependency
    {
        /// <summary>
        /// 发送通知
        /// </summary>
        /// <param name="taskid">HANGFIRE任务ID</param>
        /// <param name="fileName">上传文件名</param>
        /// <param name="realfileName">上传真实保存文件名</param>
        /// <returns></returns>
        string SendNotify(Guid id, List<string> fileName, List<string> realfileName, List<CustomCondition> customConditions);

    }
}
