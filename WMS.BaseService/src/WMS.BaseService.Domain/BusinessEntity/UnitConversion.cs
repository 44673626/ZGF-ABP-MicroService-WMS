using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.BaseService.BaseEntity;

namespace WMS.BaseService.BusinessEntity
{
    /// <summary>
    /// 单位换算关系
    /// </summary>
    [Table("WMS_UnitConversion")]
    public class UnitConversion : FullAuditedAggregateRootBase
    {
        [Comment("源单位ID，关联DictItem表（字典类型为UNIT）")]
        public int FromUnitId { get; set; }

        [Comment("目标单位ID，关联DictItem表")]
        public int ToUnitId { get; set; }

        [Comment("换算率（如1箱=24个，则FromUnit=箱，ToUnit=个，Rate=24）")]
        public decimal ConversionRate { get; set; }

        [Comment("换算关系备注（如1箱=24个）")]
        [MaxLength(500)]
        public string Remark { get; set; }
    }
}
