using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace WMS.Business.Samples
{
    public class Bom : FullAuditedAggregateRoot<Guid>
    {
        public Bom() { }
        /// <summary>
        ///年度
        /// </summary>
        public string Year { get; protected set; }

        /// <summary>
        /// 期间
        /// </summary>
        public string Period { set; get; }

        /// <summary>
        /// 工厂
        /// </summary>
        public string Factory { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string Version { set; get; }
        [Display(Name = "父物料编码")]
        [Required(ErrorMessage = "{0}是必填项")]
        public string ParentItemCode { get; set; }


        [Display(Name = "父物料描述")]
        public string ParentItemDesc { get; set; }


        [Display(Name = "组件编码")]
        [Required(ErrorMessage = "{0}是必填项")]
        public string ChildItemCode { get; set; }

        [Display(Name = "组件描述")]
        public string ChildItemDesc { get; set; }


        [Display(Name = "组件计量单位")]
        public string ChildItemUom { get; set; }


        [Display(Name = "组件数量")]
        [Required(ErrorMessage = "{0}是必填项")]
        public decimal Qty { get; set; }

        [Display(Name = "工序")]
        public int OperateProcess { get; set; }

        [Display(Name = "废品率")]
        public decimal ScrapPercent { get; set; }

        [Display(Name = "结构类型")]
        public string BomType { get; set; }

        [Display(Name = "生效时间")]
        public DateTime EffectiveTime { get; set; }

        [Display(Name = "失效时间")]
        public DateTime ExpireTime { get; set; }

        [Display(Name = "子物品消耗位置")]
        public string IssuePosition { get; set; }

        [Display(Name = "结构层级")]
        public int BomLevel { get; set; }

        public Guid ParentId { get; set; }

        public Bom(
            Guid id,
            string parentItemCode,
            string parentItemDesc,
            string childItemCode,
            string childItemDesc,
            string childItemUom,
            decimal qty,
            int operateProcess,
            decimal scrapPercent,
            string bomType,
            DateTime effectiveTime,
        DateTime expireTime,
            string issuePosition,
            int bomLevel,
            Guid parentId,
            string factory
        ) : base(id)
        {
            ParentItemCode = parentItemCode;
            ParentItemDesc = parentItemDesc;
            ChildItemCode = childItemCode;
            ChildItemDesc = childItemDesc;
            ChildItemUom = childItemUom;
            Qty = qty;
            OperateProcess = operateProcess;
            ScrapPercent = scrapPercent;
            BomType = bomType;
            EffectiveTime = effectiveTime;
            ExpireTime = expireTime;
            IssuePosition = issuePosition;
            BomLevel = bomLevel;
            ParentId = parentId;
            Factory = factory;
        }
        public void SetValue(Guid guid)
        {
            //Period = peroid;
            //Year = year;
            Id = guid;
            //Version = version;
            //ParentId = parentId;
            //Factory = factory;
        }
        public void Set(
            string parentItemDesc,
            string childItemDesc,
            string childItemUom,
            decimal qty,
            int operateProcess,
            decimal scrapPercent,
            string bomType,
            string issuePosition,
            int bomLevel)
        {
            ParentItemDesc = parentItemDesc;
            ChildItemDesc = childItemDesc;
            ChildItemUom = childItemUom;
            Qty = qty;
            OperateProcess = operateProcess;
            ScrapPercent = scrapPercent;
            BomType = bomType;
            IssuePosition = issuePosition;
            BomLevel = bomLevel;
        }
    }
}
