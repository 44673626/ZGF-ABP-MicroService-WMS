using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Values;

namespace Win.Sfs.Shared.DomainBase
{
    public class UnitQty : ValueObject
    {

        /// <summary>
        /// 存储单位
        /// </summary> 
        [Display(Name = "存储单位")]

        public string UomCode { get; set; }

        /// <summary>
        /// 存储数量
        /// </summary> 
        [Display(Name = "存储数量")]
        public decimal Qty { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {     
            yield return UomCode;
            yield return Qty;       
        }
    }
}
