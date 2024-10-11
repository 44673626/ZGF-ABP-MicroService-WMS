using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win.Sfs.Shared.DomainBase;

namespace Win.Sfs.Shared.Etos.Purchase
{
    public class AdvanceShippingNoticeDetailEto
    {
     
        /// <summary>
        /// Kanban单号
        /// </summary>
        public virtual string KanbanNumber { private set; get; }
        /// <summary>
        /// 发货单号
        /// </summary>
        public virtual string AsnNumber { private set; get; }
        /// <summary>
        /// 发货数量
        /// </summary>
        public virtual UnitQty ShipQty { private set; get; }

        /// <summary>
        /// 转换单位
        /// </summary>
        public virtual UomBase Uom { set; get; }


        /// <summary>
        /// 标包数
        /// </summary>
        public decimal StandardPackageQty { private set; get; }

        /// <summary>
        /// 供应商批次
        /// </summary>
        public virtual string VendBatch { private set; get; }
        /// <summary>
        /// 批次
        /// </summary>
        public virtual string Batch { private set; get; }
        /// <summary>
        /// 生产日期
        /// </summary>
        public virtual DateTime ProduceDate { private set; get; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public virtual string PurchaseOrderNumber { get; set; }
        /// <summary>
        /// 行号
        /// </summary>
        public virtual int LineNumber { get; set; }
        /// <summary>
        ///物品ID 
        /// </summary>
        public virtual Guid ItemId { get; set; }

        /// <summary>
        ///物品编码
        /// </summary>
        public virtual string ItemCode { get; set; }

        /// <summary>
        /// 项目ID
        /// </summary>
        public virtual Guid ProjectId { get; set; }

        /// <summary>
        /// 项目编码
        /// </summary>
        public virtual string ProjectCode { get; set; }


        /// <summary>
        /// 上级单据ID
        /// </summary>
        public virtual Guid ParentId { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }

}
