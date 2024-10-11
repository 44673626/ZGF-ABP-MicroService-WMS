using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Values;

namespace Win.Sfs.Shared.DomainBase
{
    public class Employee:ValueObject
    {
        public string Name { set; get; }
        public string Phone { set; get; }
        public string Email { set; get; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Phone;
            yield return Email;
        }
    }
}
