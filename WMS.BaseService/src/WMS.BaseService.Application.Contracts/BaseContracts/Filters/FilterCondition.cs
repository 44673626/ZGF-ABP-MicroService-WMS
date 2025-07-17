using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.BaseService.BaseContracts.Filters
{
    public class FilterCondition
    {
        public FilterCondition(string column, string value, FilterLogic logic, FilterAction action)
        {
            Column = column;
            Value = value;
            Logic = logic;
            Action = action;
        }
        public FilterCondition(string column, string value)
        {
            Column = column;
            Value = value;
            Logic = FilterLogic.And;
            Action = FilterAction.Equal;
        }

        /// <summary>
        /// 过滤条件中使用的数据列
        /// </summary>
        public string Column { get; set; }
        /// <summary>
        /// 过滤条件中的操作的值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 过滤条件之间的逻辑关系:AND和OR
        /// </summary>
        public FilterLogic Logic { get; set; }

        /// <summary>
        /// 过滤条件中的操作
        /// </summary>
        public FilterAction Action { get; set; }
        /// <summary>
        /// 子集过滤,当查询条件需要使用到"()"时,可以将括号内同一级放入子集中,以此类推
        /// </summary>
        public List<FilterCondition> Children { get; set; }
    }
}
