using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Win.Sfs.Shared.DomainBase;
using Win.Sfs.Shared.Filter;

namespace Win.Sfs.Shared.RepositoryBase
{
    public class BaseDataEfCoreRepository<TDbContext,TEntity, TKey> : EfCoreRepository<TDbContext, TEntity, TKey>,
        IBaseDataBasicRepository<TEntity, TKey> where TEntity : class, IEntity<TKey> where TDbContext : IEfCoreDbContext
    {

        public BaseDataEfCoreRepository(IDbContextProvider<TDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<long> GetCountByFilterAsync(List<FilterCondition> filters, CancellationToken cancellationToken = default)
        {

            return await this.DbSet.AsQueryable()
                .WhereIf(filters?.Count != 0, filters.ToLambda<TEntity>())
                .LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<TEntity>> GetListByFilterAsync(List<FilterCondition> filters, string sorting = null,
            int maxResultCount = int.MaxValue, int skipCount = 0, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = includeDetails ? this.WithDetails() : this.DbSet.AsQueryable();

            var entities = query.WhereIf(filters?.Count != 0, filters.ToLambda<TEntity>());

            entities = GetSortingQueryable(entities, sorting);

            return await entities.PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));


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

        public async Task<List<TKey>> GetIdsByFilterAsync(List<FilterCondition> filters, CancellationToken cancellationToken = default)
        {

            return await this.DbSet.AsQueryable()
                .WhereIf(filters?.Count != 0, filters.ToLambda<TEntity>())
                .Select(p => p.Id)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<bool> ImportAsync(List<TEntity> tList, CancellationToken cancellationToken = default)
        {
            if (tList == null || !tList.Any())
            {
                return false;
            }

            var ids = tList.Select(p => p.Id);

            var updateList = await DbSet.Where(p => ids.Contains(p.Id)).ToListAsync(cancellationToken: cancellationToken);

            var updateIds = updateList.Select(p => p.Id);

            var createList = tList.Where(p => !updateIds.Contains(p.Id));

            await DbSet.AddRangeAsync(createList, cancellationToken);

            DbSet.UpdateRange(updateList);

            await DbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> Import2Async<TSelectorKey>(Expression<Func<TEntity, TSelectorKey>> keySelector, List<TEntity> tList, CancellationToken cancellationToken = default)
        {
            if (tList == null || !tList.Any())
            {
                return false;
            }

            DbSet.AddOrUpdate(keySelector, tList);

            try
            {
                DbContext.SaveChanges();
                //await DbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteListAsync(List<TKey> ids, CancellationToken cancellationToken = default)
        {
            if (ids == null || !ids.Any())
            {
                return false;
            }

            foreach (var id in ids)
            {
                await DeleteAsync(id, false, cancellationToken);
            }

            return true;
        }
    }
}