using HangFireJob.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace HangFireJob.Samples.Interfaces
{
    public interface IExportJob : ITransientDependency
    {
        Task<string> ExportFile(Guid id, List<string> exportName, List<CustomCondition> property);
    }
}
