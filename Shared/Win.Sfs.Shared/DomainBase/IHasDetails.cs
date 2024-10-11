using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Guids;

namespace Win.Sfs.Shared.DomainBase
{
    public interface IHasDetails<TDetailEntity, TDetailKey>
    {
        /// <summary>
        /// 添加明细项
        /// </summary>
        /// <param name="guidGenerator">Guid生成器</param>
        /// <param name="detail">明细项实体</param>
        void AddDetail([NotNull] IGuidGenerator guidGenerator, TDetailEntity detail);

        /// <summary>
        /// 添加明细项列表
        /// </summary>
        /// <param name="guidGenerator">Guid生成器</param>
        /// <param name="details">明细项实体列表</param>
        void AddDetails([NotNull] IGuidGenerator guidGenerator, IEnumerable<TDetailEntity> details);

        ///// <summary>
        ///// 删除明细项
        ///// </summary>
        ///// <param name="detailKey">明细项键</param>
        //void RemoveDetail(TDetailKey detailKey);

        ///// <summary>
        ///// 删除明细项列表
        ///// </summary>
        ///// <param name="detailKeys">明细项键列表</param>
        //void RemoveDetails(List<TDetailKey> detailKeys);

        /// <summary>
        /// 判断输入明细项键是否在列表中
        /// </summary>
        /// <param name="detailKey">明细项键</param>
        /// <returns>布尔值:是否存在</returns>
        bool IsInDetails(TDetailKey detailKey);

        /// <summary>
        /// 更新明细项
        /// </summary>
        /// <param name="guidGenerator"></param>
        /// <param name="detail">明细项实体</param>
        /// <returns></returns>
        bool UpdateDetail(IGuidGenerator guidGenerator, TDetailEntity detail);

        /// <summary>
        /// 根据明细项键查找明细项
        /// </summary>
        /// <param name="id">聚合根ID</param>
        /// <param name="detailKey">明细项键</param>
        /// <returns>明细项实体</returns>
        TDetailEntity FindDetail(TDetailKey detailKey);
    }
}