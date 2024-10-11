using WMS.Business.CommonManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Business.Samples.DataDictionarys
{
    /// <summary>
    /// 构建自定义仓储
    /// </summary>
    public interface IDictionaryRepository
        : IAbpRepositoryBase<DataDictionary, Guid>, IAbpBulkRepositoryBase<DataDictionary, Guid>
    {
    }
}
