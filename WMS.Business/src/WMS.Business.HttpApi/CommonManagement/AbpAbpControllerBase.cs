using WMS.Business.CommonManagement.Caches;
using WMS.Business.CommonManagement.Crud.Inputs;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Domain.Repositories;

namespace WMS.Business.CommonManagement
{
    /// <summary>
    /// 封装控制器基类
    /// </summary>
    /// <typeparam name="TEntityDTO"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TRequestInput"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TUpdateInput"></typeparam>
    public abstract class AbpAbpControllerBase<TEntityDTO, TKey, TRequestInput, 
        TCreateInput, TUpdateInput> : AbpControllerBase,
       IAbpCrudAppService<TEntityDTO, TKey, TRequestInput, TCreateInput, TUpdateInput>
       where TEntityDTO : class, IEntityDto<TKey>
       where TRequestInput : IAbpRequest

    {
        protected IAbpCrudAppService<TEntityDTO, TKey, TRequestInput,
            TCreateInput, TUpdateInput> _service;

        public AbpAbpControllerBase(IAbpCrudAppService<TEntityDTO, TKey, 
            TRequestInput, TCreateInput, TUpdateInput> service)
        {
            _service = service;
        }

        /// <summary>
        /// 添加操作
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("")]

        public async Task<TEntityDTO> CreateAsync(TCreateInput input)
        {
            return await _service.CreateAsync(input);
        }

        /// <summary>
        /// 更新操作
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("{id}")]

        public async Task<TEntityDTO> UpdateAsync(TKey id, TUpdateInput input)
        {
            return await _service.UpdateAsync(id,input);
        }


        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]

        public async Task DeleteAsync(TKey id)
        {
            await _service.DeleteAsync(id).ConfigureAwait(false);
        }


        /// <summary>
        /// 读取缓存数据，缓存中没有，读取原始数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<TEntityDTO> GetAsync(TKey id)
        {
            return await _service.GetAsync(id).ConfigureAwait(false);
        }

        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("")]
        [RemoteService(IsMetadataEnabled = false)]
        public async Task<PagedResultDto<TEntityDTO>> GetListAsync(TRequestInput input)
        {
            return await _service.GetListAsync(input);
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
            return await _service.GetListAsync(hxRequestDTO, includeDetails, cancellationToken);
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
            return await _service.GetAllListAsync(hxRequestDTO, includeDetails, cancellationToken);
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
            return await _service.SearchAsync(keyWord, skipCount, maxResultCount, sorting, includeDetails, cancellationToken);
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
            return await _service.CountAsync(hxRequestDTO, cancellationToken);
        }

    }
}
