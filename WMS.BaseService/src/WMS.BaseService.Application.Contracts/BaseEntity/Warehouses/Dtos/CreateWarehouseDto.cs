using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WMS.BaseService.BaseContracts.Dtos;
using WMS.BaseService.BaseEnum;

namespace WMS.BaseService.BaseEntity.Warehouses.Dtos
{
    /// <summary>
    /// 创建数据DTO
    /// </summary>
    public class CreateWarehouseDto : CreateEntityDto
    {
        /// <summary>
        /// [Comment("仓库编码，唯一不可重复")]
        /// </summary>
        public string WarehouseCode { get; set; }

        /// <summary>
        /// [Comment("仓库名称")]
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// [Comment("仓库地址")]
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// [Comment("仓库面积（平方米）")]
        /// </summary>
        public decimal? Area { get; set; }

        /// <summary>
        /// [Comment("仓库负责人ID，关联Employee表的EmployeeId")]
        /// </summary>
        public int? ManagerId { get; set; }

        /// <summary>
        /// [Comment("仓库类型（1-常温/2-冷藏/3-保税）")]
        /// </summary>
        [BindNever]
        [JsonIgnore]
        public WarehouseTypeEnum WarehouseType { get; set; }

        /// <summary>
        /// [Comment("仓库状态（1-启用/0-停用）")]
        /// </summary>
        [BindNever]
        [JsonIgnore]
        public UsingStatus Status => UsingStatus.Enable;

        /// <summary>
        /// [Comment("仓库相关备注信息")]
        /// </summary>
        public string Remark { get; set; }
    }
}
