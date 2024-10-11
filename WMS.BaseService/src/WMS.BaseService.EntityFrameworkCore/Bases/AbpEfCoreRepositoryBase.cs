using EFCore.BulkExtensions;
using WMS.BaseService.CommonManagement.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;

namespace WMS.BaseService.Bases
{
    public abstract class AbpEfCoreRepositoryBase<TDbContext, TEntity, TKey>
       : AbpReadonlyEfCoreRepositoryBase<TDbContext, TEntity, TKey>, IAbpRepositoryBase<TEntity, TKey>, IAbpBulkRepositoryBase<TEntity, TKey>
       where TDbContext : IEfCoreDbContext
       where TEntity : class, IEntity<TKey>
    {
        public AbpEfCoreRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        /// <summary>
        /// 批量合并数据
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="bulkConfig"></param>
        /// <param name="progress"></param>
        /// <param name="type"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task BulkMergeAsync(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default(CancellationToken))
        {

            var context = (await GetDbContextAsync()) as DbContext;

            if (entities.Count > 0)
            {
                await context.BulkInsertOrUpdateAsync(entities, bulkConfig, progress, type, cancellationToken);
            }
        }

        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="bulkConfig"></param>
        /// <param name="progress"></param>
        /// <param name="type"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task BulkInsertAsync<TDetail>(IList<TDetail> details, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default(CancellationToken))
            where TDetail : class, new()
        {
            var context = (await GetDbContextAsync()) as DbContext;

            if (details.Count > 0)
            {
                await context.BulkInsertAsync(details, bulkConfig, progress, type, cancellationToken);
            }
        }


        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="bulkConfig"></param>
        /// <param name="progress"></param>
        /// <param name="type"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task BulkDeleteAsync(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var context = (await GetDbContextAsync()) as DbContext;

            if (entities != null && entities.Count > 0)
            {
                await context.BulkDeleteAsync(entities, bulkConfig, progress, type, cancellationToken);
            }
        }
    }
}
