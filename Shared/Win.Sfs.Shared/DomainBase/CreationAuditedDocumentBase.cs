

using System.Collections.Generic;
using Win.Sfs.Shared.Enums;

namespace Win.Sfs.Shared.DomainBase
{
    public abstract class CreationAuditedDocumentBase<TKey> : CreationAuditedEntityBase<TKey>, IDocumentNumber, IUpstreamDocument
    {
        protected CreationAuditedDocumentBase() { }

        public CreationAuditedDocumentBase(TKey id) : base(id) { }

        /// <summary>
        /// 上游单据ID
        /// </summary>
        //public TKey UpstreamDocumentId { get; set; }

        /// <summary>
        /// 单据流水号
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// 单据类型
        /// </summary>
        public EnumDocumentType DocumentType { get; set; }


        public List<UpstreamDocument> UpstreamDocuments { get; set; }

    }
}