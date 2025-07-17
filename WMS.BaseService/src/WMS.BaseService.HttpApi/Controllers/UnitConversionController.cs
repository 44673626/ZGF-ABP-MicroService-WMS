using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using WMS.BaseService.BaseEntity.UnitConversions;
using WMS.BaseService.BaseEntity.UnitConversions.Dtos;

namespace WMS.BaseService.Controllers
{
    /// <summary>
    /// 单位换算关系
    /// </summary>
    [RemoteService]
    [Area(WMSBaseRemoteServiceConsts.AreaCustomModuleName)]
    [Route($"api/{WMSBaseRemoteServiceConsts.ModuleName}/[controller]/[action]")]
    public class UnitConversionController : WMSBaseController
    {
        private readonly IUnitConversionAppService _unitConversionAppService;
        public UnitConversionController(IUnitConversionAppService unitConversionAppService)
        {
            _unitConversionAppService = unitConversionAppService;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<UnitConversionDto> CreateAsync(CreateUnitConversionDto input) => _unitConversionAppService.CreateAsync<UnitConversionDto>(input);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public Task<UnitConversionDto> ModifyAsync(ModifyUnitConversionDto input) => _unitConversionAppService.ModifyAsync<UnitConversionDto>(input);

        /// <summary>
        ///获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<UnitConversionDto> GetAsync(Guid id) => _unitConversionAppService.GetAsync<UnitConversionDto>(id);
        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<PagedResultDto<UnitConversionDto>> GetPageListAsync(RequestPageUnitConversionDto dto) => _unitConversionAppService.GetPageListAsync<UnitConversionDto>(dto);

    }
}
