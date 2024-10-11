using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseService.Systems.MessageManagement
{
    public class BasePageInput
    {
        public int PageSize { set; get; } = 100;
        /// <summary>
        /// PageIndex从0开始
        /// </summary>
        public int PageIndex { set; get; }

        /// <summary>
        /// 排序
        /// </summary>
        public string SortDirection { get; set; } = "DESC";

        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField { get; set; }
    }
}
