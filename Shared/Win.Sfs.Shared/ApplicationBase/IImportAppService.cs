using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Uow;

namespace Win.Sfs.Shared.ApplicationBase
{
    /// <summary>
    /// 导入接口
    /// </summary>
    /// <typeparam name="TCreateDto"></typeparam>
    public interface IImportAppService<TCreateDto> : IUnitOfWorkEnabled
    {
        /// <summary>
        /// 批量导入实体列表
        /// </summary>
        /// <remarks>
        /// 以ID为依据，数据库中找不到ID的实体会新增，已有ID的实体会修改
        /// </remarks>
        /// <param name="entities">实体列表</param>
        /// <returns>是否导入成功</returns>
        Task<bool> ImportAsync(List<TCreateDto> entities);
    }
}