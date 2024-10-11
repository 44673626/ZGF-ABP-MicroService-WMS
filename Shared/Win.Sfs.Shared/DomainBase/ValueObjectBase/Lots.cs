using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Values;

namespace Win.Sfs.Shared.DomainBase
{
    /// <summary>
    /// 库存批次管理
    /// </summary>
    public class Lots : ValueObject
    {

        /// <summary>
        /// 供应商批次
        /// </summary>
        public string VendBatch { set; get; }
        /// <summary>
        /// 批次
        /// </summary>
        public string ShipBatch { set; get; }

        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime ProduceDate { set; get; }


        ///// <summary>
        ///// 生产周
        ///// </summary>
        //public DateTime ProduceWeek { private set; get; }


        ///// <summary>
        ///// 到货日期
        ///// </summary>
        //public DateTime ArrivalDate { private set; get; }

        /// <summary>
        /// 收货日期
        /// </summary>
        public DateTime ReceiptDate { set; get; }



        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return VendBatch;
            yield return ShipBatch;
            yield return ProduceDate;
            yield return ReceiptDate;
        }
    }
}