using BaseService.BaseData.CompanyManagement.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace BaseService.BaseData.CompanyManagement
{
    public interface ICompanyAppService : IApplicationService
    {
        Task<PagedResultDto<CompanyDto>> GetAll(GetCompanyInputDto input);

        Task<ListResultDto<CompanyDto>> GetAllCompany();
    }
}
