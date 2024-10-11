using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;

namespace Win.Sfs.Shared.RepositoryBase
{
    public interface IWinEfCoreRepository<TEntity,TKey> 
            : IEfCoreRepository<TEntity, TKey> 
        where TEntity : class, IEntity<TKey>
    {

        Task<bool> ImportAsync(List<TEntity> tList,
            CancellationToken cancellationToken = default);

        Task<bool> Import2Async<TSelectorKey>(Expression<Func<TEntity, TSelectorKey>> keySelector, List<TEntity> tList,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteListAsync(List<TKey> ids,
            CancellationToken cancellationToken = default);

    }
}