using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.BaseService.BaseContracts.Filters
{
    public class FilterAttribute : Attribute
    {
        public string Column { get; set; }
        //public FilterAction Action { get; set; } = FilterAction.Equal;
        public FilterAction Action { get; set; } = FilterAction.Like;//进行模糊匹配
        public FilterLogic Logic { get; set; } = FilterLogic.And;
    }
}
