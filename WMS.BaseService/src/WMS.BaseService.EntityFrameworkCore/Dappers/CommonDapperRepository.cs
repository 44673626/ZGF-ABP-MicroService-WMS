using WMS.BaseService.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.Dapper;
using Volo.Abp.EntityFrameworkCore;

namespace WMS.BaseService.Dappers
{
    public class CommonDapperRepository : DapperRepository<WMSBaseDbContext>
        , ITransientDependency, ICommonDapperRepository
    {
        public CommonDapperRepository(IDbContextProvider<WMSBaseDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }


    }
}
