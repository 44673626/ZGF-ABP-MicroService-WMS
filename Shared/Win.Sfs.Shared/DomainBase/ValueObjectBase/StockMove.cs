using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Values;
using Win.Sfs.Shared.Enums;

namespace Win.Sfs.Shared.DomainBase
{
    public class StockMove : ValueObject
    {
        /// <summary>
        /// 箱码
        /// </summary>
        [Display(Name = "箱码")]
        public string InventoryCode { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        [Display(Name = "批次")]
        public string Lot { get; set; }

        /// <summary>
        /// 流水号
        /// </summary>
        [Display(Name = "流水号")]
        public string Serial { get; set; }

        /// <summary>
        /// 库存状态
        /// </summary>
        [Display(Name = "库存状态")]
        public EnumInventoryStatus InventoryStatus { get; set; }

        /// <summary>
        /// 库存位置
        /// </summary>
        public Location Location { get; set; }



        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return InventoryCode;
            yield return Lot;
            yield return Serial;
            yield return InventoryStatus;
            yield return Location;
        }
    }
}
