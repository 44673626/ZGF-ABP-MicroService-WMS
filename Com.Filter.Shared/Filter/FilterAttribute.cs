using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Filter.Shared.Filter
{
    public class FilterAttribute : Attribute
    {
        public string Column { get; set; }
        public EnumFilterAction Action { get; set; } = EnumFilterAction.Equal;
        public EnumFilterLogic Logic { get; set; } = EnumFilterLogic.And;
    }
}
