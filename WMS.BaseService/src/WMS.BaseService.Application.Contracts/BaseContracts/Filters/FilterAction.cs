using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.BaseService.BaseContracts.Filters
{
    /// <summary>
    /// 过滤条件
    /// </summary>
    public enum FilterAction
    {
        /// <summary>
        /// 等于
        /// </summary>
        [Description("等于")]
        Equal = 0,

        /// <summary>
        /// 不等于
        /// </summary>
        [Description("不等于")]
        NotEqual = 1,

        /// <summary>
        /// 大于
        /// </summary>
        [Description("大于")]
        BiggerThan = 2,

        /// <summary>
        /// 小于
        /// </summary>
        [Description("小于")]
        SmallThan = 3,

        /// <summary>
        /// 大于等于
        /// </summary>
        [Description("大于等于")]
        BiggerThanOrEqual = 4,

        /// <summary>
        /// 小于等于
        /// </summary>
        [Description("小于等于")]
        SmallThanOrEqual = 5,

        /// <summary>
        /// 类似于
        /// </summary>
        [Description("类似于")]
        Like = 6,

        /// <summary>
        /// 不类似于
        /// </summary>
        [Description("不类似于")]
        NotLike = 7,

        /// <summary>
        /// 包含于
        /// </summary>
        [Description("包含于")]
        In = 8,

        /// <summary>
        /// 不包含于
        /// </summary>
        [Description("不包含于")]
        NotIn = 9,
    }
}
