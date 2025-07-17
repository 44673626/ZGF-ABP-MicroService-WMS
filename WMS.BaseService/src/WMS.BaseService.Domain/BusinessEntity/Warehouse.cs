using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.BaseService.BaseEntity;
using WMS.BaseService.BaseEnum;

namespace WMS.BaseService.BusinessEntity
{
    /// <summary>
    /// 仓库信息
    /// </summary>
    public class Warehouse : FullAuditedAggregateRootBase
    {
        [Comment("仓库编码，唯一不可重复")]
        [MaxLength(50)]
        public string WarehouseCode { get; set; }

        [Comment("仓库名称")]
        [MaxLength(200)]
        public string WarehouseName { get; set; }

        [Comment("仓库地址")]
        [MaxLength(200)]
        public string Address { get; set; }

        [Comment("仓库面积（平方米）")]
        public decimal? Area { get; set; }

        [Comment("仓库负责人ID，关联Employee表的EmployeeId")]
        public int? ManagerId { get; set; }

        [Comment("仓库类型（1-常温/2-冷藏/3-保税）")]
        public WarehouseTypeEnum WarehouseType { get; set; }

        [Comment("仓库状态（1-启用/0-停用）")]
        public UsingStatus Status { get; set; }

        [Comment("仓库相关备注信息")]
        public string Remark { get; set; }

        /// <summary>
        /// 停用原因
        /// </summary>
        [Comment("停用原因")]
        [MaxLength(500)]
        public string StopReason { get; set; }
        /// <summary>
        /// 停用时间
        /// </summary>
        [Comment("停用时间")]
        public DateTime? StopDate { get; set; }
        /// <summary>
        /// 启用原因
        /// </summary>
        [Comment("启用原因")]
        [MaxLength(500)]
        public string StartReason { get; set; }
        /// <summary>
        /// 启用时间
        /// </summary>
        [Comment("启用时间")]
        public DateTime? StartDate { get; set; }

        #region 方法

        /// <summary>
        ///   停用
        /// </summary>
        /// <param name="startReason"></param>
        public void Disable(string stopReason)
        {
            StopReason = stopReason;
            StopDate = DateTime.Now;
            Status = UsingStatus.Disable;
        }

        /// <summary>
        ///   启用
        /// </summary>
        /// <param name="startReason"></param>
        public void Enable(string startReason)
        {
            StartReason = startReason;
            StartDate = DateTime.Now;
            Status = UsingStatus.Enable;
        }

        #endregion
    }
}
