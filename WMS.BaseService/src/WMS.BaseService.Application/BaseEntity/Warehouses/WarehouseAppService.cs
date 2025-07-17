using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using WMS.BaseService.BaseContracts.Dtos;
using WMS.BaseService.BaseEntity.Warehouses.Dtos;
using WMS.BaseService.BusinessEntity;

namespace WMS.BaseService.BaseEntity.Warehouses
{
    /// <summary>
    /// 仓库信息
    /// </summary>
    public class WarehouseAppService : WMSBaseAppService<Warehouse>, IWarehouseAppService
    {
        public WarehouseAppService(IRepository<Warehouse, Guid> repository) : base(repository)
        { 

        }

        //重写示例
        //public async override Task<ResultEntityDto> CreateAsync<ResultEntityDto>(CreateEntityDto input)
        //{
        //    var inputTemp = input as CreateWarehouseDto;
        //    inputTemp.WarehouseCode ="code";
        //    return await base.CreateAsync<ResultEntityDto>(input);
        //}

        /// <summary>
        /// 停用
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<bool> DisableAsync(UsingStatusDto dto)
        {
            var entity = await _repository.GetAsync(dto.Id);
            entity.Disable(dto.StatusChangeReason);
            await _repository.UpdateAsync(entity);
            return true;
        }

        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> EnableAsync(UsingStatusDto dto)
        {
            var entity = await _repository.GetAsync(dto.Id);
            entity.Enable(dto.StatusChangeReason);
            await _repository.UpdateAsync(entity);
            return true;
        }

    }
}
