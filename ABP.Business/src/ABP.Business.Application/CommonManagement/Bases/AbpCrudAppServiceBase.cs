using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp;
using ABP.Business.CommonManagement.Repositories;
using ABP.Business.CommonManagement.Caches;
using ABP.Business.CommonManagement.Filters;
using ABP.Business.CommonManagement.Crud.Inputs;

namespace ABP.Business.CommonManagement.Bases
{
    /// <summary>
    /// CURD服务带缓存设置
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TEntityDTO">实对应的DTO</typeparam>
    /// <typeparam name="TKey">GUID</typeparam>
    /// <typeparam name="TRequestInput">查询条件传入参数</typeparam>
    /// <typeparam name="TCreateInput">创建</typeparam>
    /// <typeparam name="TUpdateInput">更新</typeparam>
    public abstract class AbpCrudAppServiceBase<TEntity, TEntityDTO, TKey, TRequestInput, TCreateInput, TUpdateInput>
       : CrudAppService<TEntity, TEntityDTO, TKey, TRequestInput, TCreateInput, TUpdateInput>
       , IAbpCrudAppService<TEntityDTO, TKey, TRequestInput, TCreateInput, TUpdateInput>
       where TEntity : class, IEntity<TKey>
       where TEntityDTO : class, IEntityDto<TKey>
       where TRequestInput : IAbpRequest

    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly string EntityClassName = typeof(TEntity).Name;

        /// <summary>
        /// ABP内置的分布式缓存,与redis进行集成
        /// </summary>
        protected readonly IDistributedCache<TEntityDTO> Cache;

        /// <summary>
        /// 仓储
        /// </summary>
        protected readonly IAbpRepositoryBase<TEntity, TKey> _repository;

        /// <summary>
        /// 接入缓存
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="cache"></param>
        public AbpCrudAppServiceBase(IAbpRepositoryBase<TEntity, TKey> repository, IDistributedCache<TEntityDTO> cache) : base(repository)
        {
            _repository = repository;
            Cache = cache;
        }


        /// <summary>
        /// 添加，并带缓存处理
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("")]

        public override async Task<TEntityDTO> CreateAsync(TCreateInput input)
        {
            var dto = await base.CreateAsync(input);
            await Cache.SetValueAsync(dto.Id.ToString(), dto, AbpCacheConst.SeveralMinutes);
            return dto;
        }


        /// <summary>
        /// 更新，并带缓存处理
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("{id}")]

        public override async Task<TEntityDTO> UpdateAsync(TKey id, TUpdateInput input)
        {
            try
            {
                var dto = await base.UpdateAsync(id, input);
                await Cache.SetValueAsync(dto.Id.ToString(), dto, AbpCacheConst.SeveralMinutes);

                return dto;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Database operation expected to affect 1 row(s) but actually affected 0 row(s)"))
                    throw new UserFriendlyException($" {typeof(TEntityDTO).Name} 已经被他人更改, 请刷新数据后再更新.");
            }

            return null;
        }




        /// <summary>
        /// 删除，并带缓存处理
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]

        public override async Task DeleteAsync(TKey id)
        {
            await Repository.DeleteAsync(id);
            await Cache.DeleteAsync(id.ToString());
            return;
        }


        /// <summary>
        /// 读取缓存数据，缓存中没有，读取原始数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public override async Task<TEntityDTO> GetAsync(TKey id)
        {
            var dto = await Cache.GetOrAddAsync(
                id.ToString(),//缓存键
                async () => await base.GetAsync(id),
                AbpCacheConst.SeveralMinutes);

            return dto;
        }

        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("")]
        [RemoteService(IsMetadataEnabled = false)]
        public override async Task<PagedResultDto<TEntityDTO>> GetListAsync(TRequestInput input)
        {
            return await base.GetListAsync(input);
        }





        /// <summary>
        /// 按条件获取分页列表
        /// request sample
        /// {
        /// "maxResultCount": 1000,
        /// "skipCount": 0,
        /// "sorting": "",
        /// "condition": { "filters": []}
        /// }
        /// </summary>
        /// <param name="hxRequestDTO"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("list")]
        public virtual async Task<PagedResultDto<TEntityDTO>> GetListAsync(TRequestInput hxRequestDTO, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            Expression<Func<TEntity, bool>> expression = hxRequestDTO.Condition.Filters?.Count > 0
                ? hxRequestDTO.Condition.Filters.ToLambda<TEntity>()
                : p => true;
            return await GetPagedListAsync(expression, hxRequestDTO.SkipCount, hxRequestDTO.MaxResultCount, hxRequestDTO.Sorting, includeDetails, cancellationToken);
        }


        /// <summary>
        /// 按条件获取全部数据列表
        /// request sample
        /// {      
        /// "sorting": "",
        /// "condition": { "filters": []}
        /// }
        /// </summary>
        /// <param name="hxRequestDTO"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("get-all")]
        public virtual async Task<List<TEntityDTO>> GetAllListAsync(TRequestInput hxRequestDTO, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            Expression<Func<TEntity, bool>> expression = hxRequestDTO.Condition.Filters?.Count > 0
                ? hxRequestDTO.Condition.Filters.ToLambda<TEntity>()
                : p => true;
            return await GetAllListAsync(expression, hxRequestDTO.Sorting, includeDetails, cancellationToken);
        }





        /// <summary>
        /// 按表达式条件获取分页列表
        /// </summary>
        /// <param name="expression"></param>    
        /// <param name="sorting"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected async Task<List<TEntityDTO>> GetAllListAsync(Expression<Func<TEntity, bool>> expression, string sorting, bool includeDetails,
            CancellationToken cancellationToken)
        {
            var entities = await _repository.GetListAsync(expression, sorting, includeDetails, cancellationToken);
            var dtos = ObjectMapper.Map<List<TEntity>, List<TEntityDTO>>(entities);
            return dtos;
        }

        /// <summary>
        /// 按关键字获取分页列表
        /// </summary>
        /// <param name="keyWord"></param>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="sorting"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("search")]
        public virtual async Task<PagedResultDto<TEntityDTO>> SearchAsync(string keyWord, int skipCount, int maxResultCount, string sorting,
            bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            Expression<Func<TEntity, bool>> expression = p => p.Id.ToString().Contains(keyWord);
            return await GetPagedListAsync(expression, skipCount, maxResultCount, sorting, includeDetails, cancellationToken);
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
        protected async Task<PagedResultDto<TEntityDTO>> GetPagedListAsync(Expression<Func<TEntity, bool>> expression, int skipCount, int maxResultCount, string sorting, bool includeDetails,
            CancellationToken cancellationToken)
        {
            var totalCount = await _repository.GetCountAsync(expression, cancellationToken);
            var entities = await _repository.GetPagedListAsync(expression, skipCount, maxResultCount, sorting, includeDetails, cancellationToken);
            var dtos = ObjectMapper.Map<List<TEntity>, List<TEntityDTO>>(entities);
            return new PagedResultDto<TEntityDTO>(totalCount, dtos);
        }

        /// <summary>
        /// 按条件获取数量
        /// request sample
        /// {
        /// "maxResultCount": 1000,
        /// "skipCount": 0,
        /// "sorting": "",
        /// "condition": { "filters": []}
        /// }
        /// </summary>
        /// <param name="hxRequestDTO"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("count")]
        public virtual async Task<long> CountAsync(TRequestInput hxRequestDTO, CancellationToken cancellationToken = default)
        {
            Expression<Func<TEntity, bool>> expression = hxRequestDTO.Condition.Filters?.Count > 0
                ? hxRequestDTO.Condition.Filters.ToLambda<TEntity>()
                : p => true;

            var count = await _repository.GetCountAsync(expression, cancellationToken);
            return count;
        }
    }
}
