using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.BaseService.CommonManagement.Filters
{
    public class Condition
    {
        public ICollection<Filter> Filters { get; set; } = new List<Filter>();
    }
}
