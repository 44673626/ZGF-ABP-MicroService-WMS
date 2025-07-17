using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.BaseService.BaseContracts;
using WMS.BaseService.BaseContracts.Dtos;

namespace WMS.BaseService.BaseEntity.Warehouses
{
    public interface IWarehouseAppService : IWMSBaseAppService
    {
        /// <summary>
        /// 停用
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> DisableAsync(UsingStatusDto dto);
        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> EnableAsync(UsingStatusDto dto);
    }
}
