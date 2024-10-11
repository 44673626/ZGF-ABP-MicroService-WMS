using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Win.Sfs.Shared.Filter;

namespace Win.Sfs.Shared.RepositoryBase
{
    public interface IBranchEfCoreRepository<TEntity, TKey>
            : IWinEfCoreRepository<TEntity, TKey>
            , ITransientDependency
        where TEntity : class,IBranch<TKey>, IEntity<TKey>
    {
        Task<List<TEntity>> GetAllAsync(
            TKey branchId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        Task<long> GetCountAsync(
            TKey branchId,
            CancellationToken cancellationToken = default);

        Task<long> GetCountByFilterAsync(
            TKey branchId,
            List<FilterCondition> filters,
            CancellationToken cancellationToken = default);


        Task<List<TEntity>> GetListByFilterAsync(
            TKey branchId,
            List<FilterCondition> filters,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

    }
}