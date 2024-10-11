using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Values;

namespace Win.Sfs.Shared.DomainBase
{
    /// <summary>
    /// 任务数量时间
    /// </summary>
    public class TaskQty : ValueObject
    {
 
        public string TransNumber { get; set; }
        
        public decimal Qty { get; set; }
 
        public DateTime CreateTime { get; set; } 


        protected override IEnumerable<object> GetAtomicValues()
        {     
            yield return TransNumber;
            yield return Qty;
            yield return CreateTime; 
        }
    }
}
