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
    /// 库存位置
    /// </summary>
    public class Location : ValueObject
    {
        // /// <summary>
        // /// 仓库ID
        // /// </summary>
        // [Display(Name = "仓库Id")]
        // public Guid WhseId { get; set; }


        public string WhseCode { get; set; }


        // /// <summary>
        // /// 库区ID
        // /// </summary>
        // [Display(Name = "库区Id")]
        // public Guid AreaId { get; set; }
        public string AreaCode { get; set; }


        // /// <summary>
        // /// 库位组ID
        // /// </summary>
        // [Display(Name = "库位组Id")]
        // public Guid SlgId { get; set; }
        public string SlgCode { get; set; }

        ///// <summary>
        ///// 库位ID
        ///// </summary>
        //[Display(Name = "库位Id")]
        //[Required(ErrorMessage = "{0}是必填项")]
        //[StringLength(Win.Sfs.Shared.Constant.CommonConsts.MaxCodeLength, ErrorMessage = "{0}最多输入{1}个字符")]
        //public Guid LocId { get; set; }
        //public string LocCode { get; set; } 


        protected override IEnumerable<object> GetAtomicValues()
        {     
            // yield return WhseId;
            // yield return AreaId;
            // yield return SlgId;
            //yield return LocId;

            yield return WhseCode;
            yield return AreaCode;
            yield return SlgCode;
            //yield return LocCode;

        }
    }
}
