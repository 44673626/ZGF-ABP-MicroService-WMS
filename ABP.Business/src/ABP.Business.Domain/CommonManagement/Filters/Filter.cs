using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABP.Business.CommonManagement.Filters
{
    public class Filter
    {
        public Filter()
        {
            Logic = "And";
        }

        public Filter(string column, string value,
            string action = "==",
            string logic = "And")
        {
            Column = column;
            Action = action;
            Value = value;
            Logic = logic;
        }

        /// <summary>
        /// 过滤条件之间的逻辑关系:AND和OR
        /// </summary>
        public string Logic { get; set; }

        /// <summary>
        /// 过滤条件中使用的数据列
        /// </summary>
        public string Column { get; set; }

        /// <summary>
        /// 过滤条件中的操作:==,!=,>,<,>=,<=,In,NotIn,Like,NotLike
        /// Equal、NotEqual、BiggerThan、SmallThan、BiggerThanOrEqual、SmallThanOrEqual、In、NotIn
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 过滤条件中的操作的值
        /// </summary>
        public string Value { get; set; }
    }
}
