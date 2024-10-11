using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Uow;

namespace Win.Sfs.Shared.ApplicationBase
{
    /// <summary>
    /// 基础数据通用接口
    /// </summary>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IInventoryAppService<TEntityDto, TKey> : IUnitOfWorkEnabled
    {
 
        /// <summary>
        /// 获取实体总数
        /// </summary>
        /// <returns>实体总数</returns>
        Task<long> GetTotalCountAsync();

        ///// <summary>
        ///// 获取全部实体列表
        ///// </summary>
        ///// <returns>实体DTO列表</returns>
        //Task<ListResultDto<TEntityDto>> GetAllAsync();

        /// <summary>
        /// 按IDs删除实体列表
        /// </summary>
        /// <param name="ids">IDs</param>
        /// <returns>是否执行成功</returns>
        Task<bool> DeleteListAsync(List<TKey> keys);
        
        // Task DeleteByFilterAsync(string filter);
    }
}