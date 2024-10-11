using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Uow;

namespace Win.Sfs.Shared.ApplicationBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IBranchBaseDataAppService<TEntityDto, TKey>:IUnitOfWorkEnabled
    {
        // Task<TEntity> GetFromRepositoryAsync(TKey id);
        /// <summary>
        /// 获取实体总数
        /// </summary>
        /// <returns>实体总数</returns>
        Task<long> GetTotalCountAsync(TKey branchId);

        /// <summary>
        /// 获取全部实体列表
        /// </summary>
        /// <returns>实体DTO列表</returns>
        Task<ListResultDto<TEntityDto>> GetAllAsync(TKey branchId);

        /// <summary>
        /// 按IDs删除实体列表
        /// </summary>
        /// <param name="ids">IDs</param>
        /// <returns>是否执行成功</returns>
        Task<bool> DeleteListAsync(List<TKey> keys);

        // Task DeleteByFilterAsync(string filter);
    }
}