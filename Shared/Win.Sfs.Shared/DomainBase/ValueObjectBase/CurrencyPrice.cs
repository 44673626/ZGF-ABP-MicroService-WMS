using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Values;

namespace Win.Sfs.Shared.DomainBase
{
    public class CurrencyPrice:ValueObject
    {
        public decimal Price { set; get; }
        public string Currency { set; get; }
        public decimal Rate { set; get; }
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Price;
            yield return Currency;
            yield return Rate;

        }

        
    }
}
