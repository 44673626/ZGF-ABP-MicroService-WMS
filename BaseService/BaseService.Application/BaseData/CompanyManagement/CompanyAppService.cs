using BaseService.BaseData.CompanyManagement.Dto;
using BaseService.BaseData.Companys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace BaseService.BaseData.CompanyManagement
{
    /// <summary>
    /// 查询公司服务
    /// </summary>
    public class CompanyAppService : ApplicationService, ICompanyAppService
    {
        private readonly IRepository<Company, Guid> _repository;

        public CompanyAppService(IRepository<Company, Guid> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 查询公司所有列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<CompanyDto>> GetAll(GetCompanyInputDto input)
        {
            var query = (await _repository.GetQueryableAsync()).WhereIf(!string.IsNullOrWhiteSpace(input.Filter), _ => _.CompanyName.Contains(input.Filter));

            var totalCount = await query.CountAsync();
            var items = await query.OrderBy(input.Sorting ?? "CompanyCode")
                                   .Skip(input.SkipCount)
                                   .Take(input.MaxResultCount)
                                   .ToListAsync();

            var dots = ObjectMapper.Map<List<Company>, List<CompanyDto>>(items);
            return new PagedResultDto<CompanyDto>(totalCount, dots);
        }
        /// <summary>
        /// 查询所有公司列表（不分页）
        /// </summary>
        /// <returns></returns>
        public async Task<ListResultDto<CompanyDto>> GetAllCompany()
        {
            var result = await _repository.GetListAsync();
            return new ListResultDto<CompanyDto>(ObjectMapper.Map<List<Company>, List<CompanyDto>>(result));
        }

    }
}
