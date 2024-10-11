using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Guids;
using Win.Sfs.Shared.DomainBase;

namespace Win.Sfs.Shared.Etos.Purchase
{
    public class AdvanceShippingNoticeEto 
    {
        /// <summary>
        /// 分支ID
        /// </summary>
        public Guid BranchId { get; set; }

        /// <summary>
        /// 发货单号
        /// </summary>
        public virtual string AsnNumber { private set; get; }
        /// <summary>
        /// 看板编号
        /// </summary>
        public virtual string KanbanNumber { private set; get; }
        public int State { get; set; }
        /// <summary>
        /// 收货仓库ID
        /// </summary>
        public Guid ReceiptWhseId { set; get; }

        /// <summary>
        /// 收货口ID
        /// </summary>
        public Guid ReceiptPortId { set; get; }

        /// <summary>
        /// 收货仓库Code
        /// </summary>
        public string ReceiptWhseCode { set; get; }

        /// <summary>
        /// 收货口Code
        /// </summary>
        public string ReceiptPortCode { set; get; }


        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }


        public virtual ICollection<AdvanceShippingNoticeDetailEto> Details { set; get; }

      
        /// <summary>
        /// 计划时间（到货时间）时间窗口开始
        /// </summary>
        public virtual DateTime PlanTimeBegin { private set; get; }
        /// <summary>
        /// 计划时间（到货时间）时间窗口结束
        /// </summary>
        public virtual DateTime PlanTimeEnd { private set; get; }
        /// <summary>
        /// 发货日期
        /// </summary>
        public virtual DateTime ShipDate { private set; get; }
       
        public virtual Employee ShipUser {  set; get; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string PurchaseOrderNumber { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public Guid SupplierId { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public string SupplierCode { get; set; }


        /// <summary>
        /// 父节点ID
        /// </summary>
        public Guid ParentId { get; set; }
        /// <summary>
        /// 根节点ID
        /// </summary>
        public Guid RootId { get; set; }
        public int OrderType { get; set; }
    }
}
