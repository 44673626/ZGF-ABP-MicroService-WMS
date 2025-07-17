using WMS.BaseService.Localization;
using Volo.Abp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Threading.Tasks;
using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using WMS.BaseService.BaseContracts.Dtos;
using WMS.BaseService.BaseContracts;
using WMS.BaseService.BaseContracts.Filters;

namespace WMS.BaseService;

public class WMSBaseAppService : ApplicationService, IApplicationService
{
    public WMSBaseAppService()
    {
        LocalizationResource = typeof(WMSBaseResource);
    }
}

public class WMSBaseAppService<T> : ApplicationService, IApplicationService,IWMSBaseAppService where T : class, IHasConcurrencyStamp, IEntity<Guid>
{
    protected readonly IRepository<T, Guid> _repository;

    public WMSBaseAppService(IRepository<T, Guid> repository)
    {
        LocalizationResource = typeof(WMSBaseResource);
        _repository = repository;
    }
    public virtual async Task<ResultEntityDto> CreateAsync<ResultEntityDto>(CreateEntityDto input)
    {
        var entity = ObjectMapper.Map<CreateEntityDto, T>(input);
        await _repository.InsertAsync(entity);
        return ObjectMapper.Map<T, ResultEntityDto>(entity);
    }

    public virtual async Task<bool> CreateManyAsync(List<CreateEntityDto> inputs)
    {
        var entities = new List<T>();
        await _repository.InsertManyAsync(entities);
        return true;
    }

    public virtual async Task<ResultEntityDto> GetAsync<ResultEntityDto>(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var result = ObjectMapper.Map<T, ResultEntityDto>(entity);
        return result;
    }

    public virtual async Task<List<ResultEntityDto>> GetListAsync<ResultEntityDto>(RequestEntityDto dto)
    {
        IQueryable<T> queryable;
        if (dto.IncludeDetails)
            queryable = await _repository.WithDetailsAsync();
        else
            queryable = await _repository.GetQueryableAsync();
        var filters = dto.ToFilters();
        queryable = queryable.WhereIf(filters.Count > 0, filters.ToExpression<T>());
        var entities = await queryable.ToDynamicListAsync<T>();
        var results = ObjectMapper.Map<List<T>, List<ResultEntityDto>>(entities);
        return results;
    }

    public virtual async Task<ResultEntityDto> ModifyAsync<ResultEntityDto>(ModifyEntityDto input)
    {
        var entity = await _repository.GetAsync(input.Id);
        ObjectMapper.Map(input, entity);
        await _repository.UpdateAsync(entity, true);
        return ObjectMapper.Map<T, ResultEntityDto>(entity);
    }
    public virtual async Task<ResultEntityDto> ModifyAsync<ResultEntityDto>(ModifyEntityDto input,
        Action<ModifyEntityDto, T> overMapper = null)
    {
        var entity = await _repository.GetAsync(input.Id);
        ObjectMapper.Map(input, entity);
        if (overMapper != null)
            overMapper(input, entity);
        await _repository.UpdateAsync(entity, true);
        return ObjectMapper.Map<T, ResultEntityDto>(entity);
    }

    public virtual async Task<bool> ModifyManyAsync(List<ModifyEntityDto> inputs)
    {
        var entities = await _repository.GetListAsync(r => inputs.Select(c => c.Id).Contains(r.Id));
        foreach (var entity in entities)
        {
            var item = inputs.FirstOrDefault(r => r.Id == entity.Id);
            if (item != null)
                ObjectMapper.Map(item, entity);
        }
        await _repository.UpdateManyAsync(entities);
        return true;
    }

    public virtual async Task<bool> ModifyManyAsyn(List<ModifyEntityDto> inputs,
       Action<ModifyEntityDto, T> overMapper = null)
    {
        var entities = await _repository.GetListAsync(r => inputs.Select(c => c.Id).Contains(r.Id));
        foreach (var entity in entities)
        {
            var item = inputs.FirstOrDefault(r => r.Id == entity.Id);
            if (item != null)
                ObjectMapper.Map(item, entity);
            if (overMapper != null)
                overMapper(item, entity);
        }
        await _repository.UpdateManyAsync(entities);
        return true;
    }

    public virtual async Task<bool> DeleteAsync(DeleteEntityDto input)
    {
        var entity = await _repository.GetAsync(input.Id);
        await _repository.DeleteAsync(entity);
        return true;
    }

    public virtual async Task<bool> DeleteManyAsync([FromBody] List<DeleteEntityDto> inputs)
    {
        var entities = await _repository.GetListAsync(r => inputs.Select(c => c.Id).Contains(r.Id));
        // ObjectMapper.Map(inputs, entities);
        await _repository.DeleteManyAsync(entities);
        return true;
    }

    public virtual async Task<PagedResultDto<ResultEntityDto>> GetPageListAsync<ResultEntityDto>(RequestPageEntityDto input)
    {
        var queryable = input.IncludeDetails ? await _repository.WithDetailsAsync() : await _repository.GetQueryableAsync();
        var filters = input.ToFilters();
        queryable = queryable.WhereIf(filters.Count > 0, filters.ToExpression<T>());
        queryable = DynamicQueryableExtensions.OrderBy(queryable, input.Sorting!);
        var entities = await queryable.Skip(input.SkipCount).Take(input.MaxResultCount).ToDynamicListAsync<T>();
        var results = ObjectMapper.Map<List<T>, List<ResultEntityDto>>(entities);
        return new PagedResultDto<ResultEntityDto>()
        {
            TotalCount = queryable.Count(),
            Items = results
        };
    }

    public async Task<PagedResultDto<ResultEntityDto>> GetPageListAsync<ResultEntityDto>(RequestPageEntityDto input, Func<IQueryable<T>, IQueryable<T>>? query = null)
    {
        var queryable = input.IncludeDetails ? await _repository.WithDetailsAsync() : await _repository.GetQueryableAsync();
        var filters = input.ToFilters();
        queryable = queryable.WhereIf(filters.Count > 0, filters.ToExpression<T>());
        queryable = DynamicQueryableExtensions.OrderBy(queryable, input.Sorting!);
        if (query != null)
        {
            queryable = query(queryable);
        }
        var entities = await queryable.Skip(input.SkipCount).Take(input.MaxResultCount).ToDynamicListAsync<T>();
        var results = ObjectMapper.Map<List<T>, List<ResultEntityDto>>(entities);
        return new PagedResultDto<ResultEntityDto>()
        {
            TotalCount = queryable.Count(),
            Items = results
        };
    }

}
