using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Win.Sfs.Shared.RepositoryBase
{
    public interface IBaseDataExtentRepository<TEntity>
    {
        Task<List<TEntity>> GetListWithDetailAsync(
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string filter = null,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);
    }
}