using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Values;

namespace Win.Sfs.Shared.DomainBase
{
    public class UomBase:ValueObject
    {
        public decimal UomConversionValue { set; get; }
        public string UomCode { set; get; }
         

        protected override IEnumerable<object> GetAtomicValues()
        {     
            yield return UomConversionValue;
            yield return UomCode;       
        }
    }
}
