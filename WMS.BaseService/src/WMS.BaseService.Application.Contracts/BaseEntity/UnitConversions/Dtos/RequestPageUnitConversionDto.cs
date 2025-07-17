using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.BaseService.BaseContracts.Dtos;
using WMS.BaseService.BaseContracts.Filters;

namespace WMS.BaseService.BaseEntity.UnitConversions.Dtos
{
    public class RequestPageUnitConversionDto : RequestPageEntityDto
    {
        /// <summary>
        /// 编码
        /// </summary>
        [Filter(Action = FilterAction.Like)]
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Filter(Action = FilterAction.Like)]
        public string Name { get; set; }
        /// <summary>
        /// 0停用、1启用状态
        /// </summary>
        public int Status { get; set; }
    }
}
