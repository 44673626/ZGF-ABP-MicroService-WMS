using WMS.BaseService.CommonManagement.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace WMS.BaseService.CommonManagement.Crud.Inputs
{
    public class AbpRequestInputBase : PagedAndSortedResultRequestDto, IAbpRequest
    {
        /// <summary>
        /// 条件
        /// </summary>
        public Condition Condition { get; set; } = new();

    }
}
