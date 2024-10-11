using WMS.Business.Bases;
using WMS.Business.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;

namespace WMS.Business.Samples.DataDictionarys
{
    public class DictionaryEfCoreRepository
        : AbpAuthEfCoreRepositoryBase<ABPVNextDbContext, DataDictionary, Guid>, IDictionaryRepository
    {
        public DictionaryEfCoreRepository(IDbContextProvider<ABPVNextDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
