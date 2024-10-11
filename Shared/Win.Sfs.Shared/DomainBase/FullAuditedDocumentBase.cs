using System.Collections.Generic;
using Win.Sfs.Shared.Enums;

namespace Win.Sfs.Shared.DomainBase
{
    public abstract class FullAuditedDocumentBase<TKey> : FullAuditedAggregateRootBase<TKey>, IDocumentNumber, IUpstreamDocument
    {
        protected FullAuditedDocumentBase() { }

        public FullAuditedDocumentBase(TKey id) : base(id) { }

        /// <summary>
        /// 单据流水号
        /// </summary>
        public string DocumentNumber { get; set; }


        /// <summary>
        /// 单据类型
        /// </summary>
        public EnumDocumentType DocumentType { get; set; }


        /// <summary>
        /// 状态
        /// </summary>
        public EnumDocumentStatus DocumentStatus { get; set; }

        public List<UpstreamDocument> UpstreamDocuments { get; set; }
 
    }
}