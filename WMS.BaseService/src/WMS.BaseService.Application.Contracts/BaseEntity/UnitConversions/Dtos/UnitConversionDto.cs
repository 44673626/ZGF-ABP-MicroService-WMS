using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.BaseService.BaseContracts.Dtos;

namespace WMS.BaseService.BaseEntity.UnitConversions.Dtos
{
    public class UnitConversionDto : ResultEntityDto
    {
        /// <summary>
        /// [Comment("源单位ID，关联DictItem表（字典类型为UNIT）")]
        /// </summary>
        public int FromUnitId { get; set; }

        /// <summary>
        /// [Comment("目标单位ID，关联DictItem表")]
        /// </summary>
        public int ToUnitId { get; set; }

        /// <summary>
        /// [Comment("换算率（如1箱=24个，则FromUnit=箱，ToUnit=个，Rate=24）")]
        /// </summary>
        public decimal ConversionRate { get; set; }

        /// <summary>
        /// [Comment("换算关系备注（如1箱=24个）")]
        /// </summary>
        public string Remark { get; set; }
    }
}
