using HangFireJob.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.DependencyInjection;

namespace HangFireJob.Samples.Interfaces
{
    public interface IImportJob : ITransientDependency
    {
        /// <summary>
        /// 导入文件
        /// </summary>
        /// <param name="taskid">hangefire任务ID</param>
        /// <param name="fileName"></param>
        /// <param name="realfileName"></param>
        /// <returns></returns>
        string ImportFile(Guid taskid, List<string> fileName, List<string> realfileName, List<CustomCondition> customConditions);
    }
}
