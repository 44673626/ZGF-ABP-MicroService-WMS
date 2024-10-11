using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ABP.Business.Bases
{
    public abstract class AbpReadonlyEfCoreRepositoryBase<TDbContext, TEntity, TKey>
       : EfCoreRepository<TDbContext, TEntity, TKey>, IAbpReadonlyRepositoryBase<TEntity, TKey>
       where TDbContext : IEfCoreDbContext
       where TEntity : class, IEntity<TKey>
    {
        protected ILoggerFactory LoggerFactory => this.LazyServiceProvider.LazyGetRequiredService<ILoggerFactory>();
        protected ILogger Logger => this.LazyServiceProvider.LazyGetService<ILogger>(_ => (object)this.LoggerFactory?.CreateLogger(this.GetType().FullName) ?? NullLogger.Instance);

        protected AbpReadonlyEfCoreRepositoryBase(
            IDbContextProvider<TDbContext> dbContextProvider
            ) : base(dbContextProvider)
        {

        }


        /// <summary>
        /// 按表达式条件获取分页列表
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="sorting"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> expression,
            int skipCount, int maxResultCount, string sorting,
            bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var query = includeDetails ? await WithDetailsAsync() : await GetDbSetAsync();

            var entities = query.Where(expression);
            entities = GetSortingQueryable(entities, sorting);
            var str = entities.ToQueryString();

            Logger.LogDebug(str);

            var result = await entities.PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));

            return result;

        }

        /// <summary>
        /// 按表达式条件获取数量
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<long> GetCountAsync(Expression<Func<TEntity, bool>> expression,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            var query = dbSet.AsQueryable().Where(expression);
            var count = await query.LongCountAsync(GetCancellationToken(cancellationToken));
            return count;
        }

        /// <summary>
        /// 按表达式条件获取列表
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="sorting"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression,
            string sorting, bool includeDetails = false, CancellationToken cancellationToken = default)
        {

            var query = includeDetails ? await WithDetailsAsync() : await GetDbSetAsync();

            var entities = query.Where(expression);

            entities = GetSortingQueryable(entities, sorting);
            var str = entities.ToQueryString();

            Logger.LogDebug(str);

            var result = await entities.ToListAsync(GetCancellationToken(cancellationToken));
            return result;
        }

        private static IQueryable<TEntity> GetSortingQueryable(IQueryable<TEntity> entities, string sorting)
        {
            if (string.IsNullOrEmpty(sorting))
            {
                entities = entities.OrderByDescending("Id");
            }
            else
            {
                var sortParams = sorting?.Split(' ');
                var sortName = sortParams[0];
                bool isDesc;
                if (sortParams.Length > 1)
                {
                    var sortDirection = sortParams[1];
                    isDesc = sortDirection == "DESC";
                }
                else
                {
                    isDesc = true;
                }

                entities = isDesc ? entities.OrderByDescending(sortName) : entities.OrderBy(sortName);
            }

            return entities;
        }

    }
}
