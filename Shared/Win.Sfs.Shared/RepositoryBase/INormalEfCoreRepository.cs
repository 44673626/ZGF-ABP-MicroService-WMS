using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Win.Sfs.Shared.Filter;

namespace Win.Sfs.Shared.RepositoryBase
{

    public interface INormalEfCoreRepository<TEntity, TKey>
        : IWinEfCoreRepository<TEntity, TKey>
            , ITransientDependency
        where TEntity : class, IEntity<TKey>
    {
        Task<List<TEntity>> GetAllAsync(
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        Task<long> GetCountByFilterAsync(
            List<FilterCondition> filters,
            CancellationToken cancellationToken = default);

        Task<List<TEntity>> GetListByFilterAsync(
            List<FilterCondition> filters,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

    }
}