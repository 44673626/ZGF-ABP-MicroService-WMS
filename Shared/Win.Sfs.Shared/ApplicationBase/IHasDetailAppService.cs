using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Uow;

namespace Win.Sfs.Shared.ApplicationBase
{
    public interface IHasDetailAppService<TKey, TDetailEntity, TDetailRequestDto, TDetailKey> : IUnitOfWorkEnabled
    {
        Task<TDetailEntity> GetDetailAsync(TKey id, TDetailKey detailKey);

        Task<List<TDetailEntity>> GetAllDetailsAsync(TKey id);

        Task<long> GetDetailCountAsync(TKey id);

        Task<List<TDetailEntity>> GetDetailsByFilterAsync(TKey key, TDetailRequestDto input);

        // Task<bool> AddDetailAsync(TKey id, TDetailEntity detail);

        Task<bool> AddDetailsAsync(TKey id, List<TDetailEntity> details);

        Task<bool> ClearDetailsAsync(TKey id);

        // Task<bool> UpdateDetailAsync(TKey id, TDetailEntity detail);

        Task<bool> UpdateDetailsAsync(TKey key, List<TDetailEntity> details);

        // Task<bool> DeleteDetailAsync(TKey id, TDetailKey detailKey);

        Task<bool> DeleteDetailsAsync(TKey key, List<TDetailKey> detailKeys);
    }
}