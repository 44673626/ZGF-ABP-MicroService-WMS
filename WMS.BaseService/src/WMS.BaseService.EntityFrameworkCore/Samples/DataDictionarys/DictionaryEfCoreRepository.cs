using WMS.BaseService.Bases;
using WMS.BaseService.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;

namespace WMS.BaseService.Samples.DataDictionarys
{
    public class DictionaryEfCoreRepository
        : AbpAuthEfCoreRepositoryBase<WMSBaseDbContext, DataDictionary, Guid>, IDictionaryRepository
    {
        public DictionaryEfCoreRepository(IDbContextProvider<WMSBaseDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
