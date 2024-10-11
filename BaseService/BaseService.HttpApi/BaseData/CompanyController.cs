using BaseService.BaseData.CompanyManagement.Dto;
using BaseService.BaseData.CompanyManagement;
using BaseService.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace BaseService.HttpApi.BaseData
{
    [Area("base")]
    [Route("api/base/company")]
    //[ApiExplorerSettings(GroupName = "base")]
    public class CompanyController : BaseServiceController, ICompanyAppService
    {
        private readonly ICompanyAppService _companyAppService;

        public CompanyController(ICompanyAppService companyAppService)
        {
            _companyAppService = companyAppService;
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("all")]
        public Task<PagedResultDto<CompanyDto>> GetAll(GetCompanyInputDto input)
        {
            return _companyAppService.GetAll(input);
        }

        [HttpGet]
        [Route("getcompanyall")]
        public Task<ListResultDto<CompanyDto>> GetAllCompany()
        {
            return _companyAppService.GetAllCompany();
        }

    }
}
