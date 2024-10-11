using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Business.Samples.Boms.Dto
{
    /// <summary>
    /// 导入Bom数据Dto
    /// IsLabelingError：是否标注数据错误
    /// </summary>
    //[ImportProject(Name = SettleAccountModuleName.Bom)]
    [ExcelImporter(IsLabelingError = true, MaxCount = int.MaxValue,HeaderRowIndex =5)]
    public class ImportBomDto
    {
        [ImporterHeader(Name = "父物料号", FixAllSpace = true)]
        [Required(ErrorMessage = "{0}是必填项")]
        [MaxLength(300, ErrorMessage = "{0}最多输入{1}个字符")]
        public string ParentItemCode { get; set; }

        [ImporterHeader(Name = "父物料描述")]
        [MaxLength(300, ErrorMessage = "{0}最多输入{1}个字符")]
        public string ParentItemDesc { get; set; }

        [ImporterHeader(Name = "组件", FixAllSpace = true)]
        [Required(ErrorMessage = "{0}是必填项")]
        [MaxLength(300, ErrorMessage = "{0}最多输入{1}个字符")]
        public string ChildItemCode { get; set; }

        [ImporterHeader(Name = "组件描述")]
        [MaxLength(300, ErrorMessage = "{0}最多输入{1}个字符")]
        public string ChildItemDesc { get; set; }

        [ImporterHeader(Name = "组件数量")]
        [Required(ErrorMessage = "{0}是必填项")]

        public decimal Qty { get; set; }


        [ImporterHeader(Name = "组件计量单位")]
        [MaxLength(300, ErrorMessage = "{0}最多输入{1}个字符")]

        public string ChildItemUom { get; set; }

        [Display(Name = "工序")]
        [Required(ErrorMessage = "{0}是必填项")]
        public int OperateProcess { get; set; }

        [Display(Name = "废品率")]
        public decimal ScrapPercent { get; set; }

        [Display(Name = "结构类型")]
        [StringLength(300, ErrorMessage = "{0}最多输入{1}个字符")]
        public string BomType { get; set; }

        [Display(Name = "生效时间")]
        public DateTime EffectiveTime { get; set; }

        [Display(Name = "失效时间")]
        public DateTime ExpireTime { get; set; }

        [Display(Name = "子物品消耗位置")]
        public string IssuePosition { get; set; }

        [Display(Name = "结构层级")]
        public int BomLevel { get; set; }

    }
}
