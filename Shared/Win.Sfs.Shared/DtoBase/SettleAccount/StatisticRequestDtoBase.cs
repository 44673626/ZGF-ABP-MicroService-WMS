using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Win.Sfs.Shared;
using Win.Sfs.Shared.DtoBase;
using Win.Sfs.Shared.Filter;

namespace Win.Sfs.Shared.DtoBase.SettleAccount
{
    /// <summary>
    /// 查询条件 DTO 
    /// </summary>
    public class StatisticRequestDtoBase : BranchRequestDtoBase
    {
        /// <summary>
        ///  年度
        /// </summary>
        [Display(Name = "年度")]
        public string Year { set; get; }

        /// <summary>
        /// 期间
        /// </summary>
        [Display(Name = "期间")]
        public string Period { set; get; }


        /// <summary>
        /// 版本
        /// </summary>
        [Display(Name = "版本")]
        public string Version { set; get; }

        /// <summary>
        /// 客户编码
        /// </summary>
     
        [Display(Name = "客户编码")]
        public string CustomerCode { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [Display(Name = "客户名称")]
        public string CustomerName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        public string Remark { get; set; }


        ///// <summary>
        ///// 是否导出文件
        ///// </summary>
        //[Display(Name = "是否导出文件")]
        //public bool IsExport { get; set; } 

    }
}