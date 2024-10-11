using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Business.CommonManagement.Filters
{
    /// <summary>
    /// 过滤条件
    /// </summary>
    public enum EnumFilterAction
    {
        /// <summary>
        /// equal
        /// </summary>
        [Description("等于")] Equal = 0,

        /// <summary>
        /// Not equal
        /// </summary>
        [Description("不等于")] NotEqual = 1,

        /// <summary>
        /// Bigger
        /// </summary>
        [Description("大于")] BiggerThan = 2,

        /// <summary>
        /// Smaller
        /// </summary>
        [Description("小于")] SmallThan = 3,

        /// <summary>
        /// Bigger or equal
        /// </summary>
        [Description("大于等于")] BiggerThanOrEqual = 4,

        /// <summary>
        /// Small or equal
        /// </summary>
        [Description("小于等于")] SmallThanOrEqual = 5,

        /// <summary>
        /// Like
        /// </summary>
        [Description("类似于")] Like = 6,

        /// <summary>
        /// Not like
        /// </summary>
        [Description("不类似于")] NotLike = 7,

        /// <summary>
        /// Contained in
        /// List<string > items = new List<string>();
        /// string value = JsonSerializer.Serialize(items);//转成Json字符串
        ///FilterCondition filterCondition = new FilterCondition() { Column = "Name", Value = value, Action = EnumFilterAction.In, Logic = EnumFilterLogic.And };
        /// </summary>
        [Description("包含于")] In = 8,

        /// <summary>
        /// Not contained in
        /// </summary>
        [Description("不包含于")] NotIn = 9,
    }
}
