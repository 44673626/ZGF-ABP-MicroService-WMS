using AuthServer.Host.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.Dapper;
using Volo.Abp.EntityFrameworkCore;
using Dapper;
using System.Linq;

namespace AuthServer.Host.Dappers
{
    public class CompanyDapperRepository : DapperRepository<AuthServerDbContext>
        , ITransientDependency
    {
        public CompanyDapperRepository(IDbContextProvider<AuthServerDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }
        public virtual async Task<List<CompanyInfo>> GetUserCompanyIds(Guid userid)
        {
            var sql = @"SELECT CompanyId, CompanyName FROM [dbo].[base_user_companys] a
                         LEFT JOIN base_company b on a.CompanyId = b.Id
                         WHERE UserId = @UserId"; // 使用参数化查询
            return (await (await GetDbConnectionAsync())
                    .QueryAsync<CompanyInfo>(
                        sql, new { UserId = userid },
                        transaction: await GetDbTransactionAsync()
                    )
                ).ToList();
        }
    }
}
