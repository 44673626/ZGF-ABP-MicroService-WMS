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
    /// <summary>
    /// 创建 单元换算关系
    /// </summary>
    public class CreateUnitConversionDto : CreateEntityDto
    {
        public int FromUnitId { get; set; }

        public int ToUnitId { get; set; }

        public decimal ConversionRate { get; set; }

    }
}
