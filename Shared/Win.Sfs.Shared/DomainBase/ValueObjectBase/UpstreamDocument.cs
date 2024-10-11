using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Values;
using Win.Sfs.Shared.Enums;

namespace Win.Sfs.Shared.DomainBase
{
    public class UpstreamDocument : ValueObject
    { 


        /// <summary>
        /// 上游单据ID
        /// </summary>
        public Guid UpstreamDocumentId { get; set; }

        /// <summary>
        /// 上游单据Number
        /// </summary>
        public string UpstreamDocumentNumber { get; set; }

        /// <summary>
        /// 上游单据类型
        /// </summary>
        public EnumDocumentType UpstreamDocumentType { get; set; }



        /// <summary>
        /// 排序序号
        /// </summary>
        public int Seq { get; set; }



        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return UpstreamDocumentId;
            yield return UpstreamDocumentNumber;
            yield return UpstreamDocumentType;
            yield return Seq;

        }

        
    }
}
