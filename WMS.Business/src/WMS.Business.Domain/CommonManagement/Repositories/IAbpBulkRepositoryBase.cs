using EFCore.BulkExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace WMS.Business.CommonManagement.Repositories
{
    /// <summary>
    /// 批量操作数据
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IAbpBulkRepositoryBase<TEntity, TKey>
          where TEntity : class, IEntity<TKey>
    {

        /// <summary>
        /// 批量合并数据
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="bulkConfig"></param>
        /// <param name="progress"></param>
        /// <param name="type"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task BulkMergeAsync(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="bulkConfig"></param>
        /// <param name="progress"></param>
        /// <param name="type"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task BulkDeleteAsync(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="bulkConfig"></param>
        /// <param name="progress"></param>
        /// <param name="type"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task BulkInsertAsync<TDetail>(IList<TDetail> details, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default(CancellationToken))
            where TDetail : class, new();
    }
}
