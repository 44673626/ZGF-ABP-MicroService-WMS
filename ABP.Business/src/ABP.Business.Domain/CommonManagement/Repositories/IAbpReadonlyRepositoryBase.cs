using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace ABP.Business.CommonManagement.Repositories
{
    /// <summary>
    /// 通用查询接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IAbpReadonlyRepositoryBase<TEntity, TKey> : IReadOnlyRepository<TEntity, TKey>, ITransientDependency
     where TEntity : class, IEntity<TKey>
    {


        Task<List<TEntity>> GetPagedListAsync(
            Expression<Func<TEntity, bool>> expression,
            int skipCount,
            int maxResultCount,
            string sorting,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default(CancellationToken));

        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression, string sorting, bool includeDetails = false, CancellationToken cancellationToken = default);
    }
}
