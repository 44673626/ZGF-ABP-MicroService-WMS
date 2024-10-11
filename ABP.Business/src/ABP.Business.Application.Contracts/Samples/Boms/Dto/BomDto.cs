using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ABP.Business.Samples.Boms.Dto
{
    public class BomDto : AuditedEntityDto<Guid>
    {

        [Display(Name = "父物料编码")]
        [Required(ErrorMessage = "{0}是必填项")]
        public string ParentItemCode { get; set; }


        [Display(Name = "父物料描述")]
        [MaxLength(300, ErrorMessage = "{0}最多输入{1}个字符")]
        public string ParentItemDesc { get; set; }


        [Display(Name = "组件编码")]
        [Required(ErrorMessage = "{0}是必填项")]
        public string ChildItemCode { get; set; }

        [Display(Name = "组件描述")]
        [MaxLength(300, ErrorMessage = "{0}最多输入{1}个字符")]
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
        [StringLength(36, ErrorMessage = "{0}最多输入{1}个字符")]
        public string BomType { get; set; }

        [Display(Name = "生效时间")]
        public DateTime EffectiveTime { get; set; }

        [Display(Name = "失效时间")]
        public DateTime ExpireTime { get; set; }

        [Display(Name = "子物品消耗位置")]
        public string IssuePosition { get; set; }

        [Display(Name = "结构层级")]
        public int BomLevel { get; set; }


        public string Version { set; get; }
        /// <summary>
        /// 工厂
        /// </summary>
        public string Factory { set; get; }




    }
}
