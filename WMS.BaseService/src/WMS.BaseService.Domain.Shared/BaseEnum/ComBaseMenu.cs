using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.BaseService.BaseEnum
{
    public enum WarehouseTypeEnum
    {
        [Display(Name = "常温")]
        CW =1,
        [Display(Name = "冷藏")]
        LC =2,
        [Display(Name = "保税")]
        BS =3
    }
    /// <summary>
    /// 停用/启用状态
    /// </summary>
    public enum UsingStatus
    {
        /// <summary>
        /// 停用
        /// </summary>
        [Display(Name = "停用")]
        Disable = 0,
        /// <summary>
        /// 启用
        /// </summary>
        [Display(Name = "启用")]
        Enable = 1
    }
    /// <summary>
    /// 通用是否状态
    /// </summary>
    public enum YerOrNo
    {
        /// <summary>
        /// 否
        /// </summary>
        [Display(Name = "否")]
        No = 0,
        /// <summary>
        /// 是
        /// </summary>
        [Display(Name = "是")]
        Yes = 1
    }
}
