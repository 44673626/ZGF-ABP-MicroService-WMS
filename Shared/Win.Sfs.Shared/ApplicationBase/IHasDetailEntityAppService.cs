using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Uow;

namespace Win.Sfs.Shared.ApplicationBase
{
    public interface IHasDetailEntityAppService<TKey, TDetailEntity, TDetailRequestDto, TDetailKey> : IUnitOfWorkEnabled
    {
        Task<TDetailEntity> GetDetailAsync(TDetailKey detailKey);

        Task<List<TDetailEntity>> GetAllDetailsAsync(TKey id);

        Task<long> GetDetailCountAsync(TKey id);

        Task<PagedResultDto<TDetailEntity>> GetDetailsByFilterAsync(TDetailRequestDto input);

        //Task<bool> AddDetailsAsync(TKey id, List<TDetailEntity> details);

        //Task<bool> ClearDetailsAsync(TKey id);

        //// Task<bool> UpdateDetailAsync(TKey id, TDetailEntity detail);

        //Task<bool> UpdateDetailsAsync(TKey key, List<TDetailEntity> details);

        //// Task<bool> DeleteDetailAsync(TKey id, TDetailKey detailKey);

        //Task<bool> DeleteDetailsAsync(TKey key, List<TDetailKey> detailKeys);
    }
}