using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.BaseService.BaseContracts.Dtos;

namespace WMS.BaseService.BaseEntity.UnitConversions.Dtos
{
    /// <summary>
    /// 修改Dto
    /// </summary>
    public class ModifyUnitConversionDto : ModifyEntityDto
    {
        public int FromUnitId { get; set; }

        public int ToUnitId { get; set; }

        public decimal ConversionRate { get; set; }
    }
}
