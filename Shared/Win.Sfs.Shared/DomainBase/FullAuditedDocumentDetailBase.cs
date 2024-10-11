using Win.Sfs.Shared.Enums;

namespace Win.Sfs.Shared.DomainBase
{
    public abstract class FullAuditedDocumentDetailBase<TKey> : FullAuditedEntityBase<TKey>,IDocumentNumber,IItem<TKey>
    {
        protected FullAuditedDocumentDetailBase() { }

        public FullAuditedDocumentDetailBase(TKey id) : base(id) { }

        /// <summary>
        /// 行号
        /// </summary>
        public int LineNumber { get; set; } = 0;

        /// <summary>
        /// 单据ID
        /// </summary>
        public TKey DocumentId { get; set; }

        /// <summary>
        /// 单据流水号
        /// </summary>
        public string DocumentNumber { get; set; }


        /// <summary>
        /// 明细状态
        /// </summary>
        public EnumDetailStatusType DetailStatus { get; set; }

        /// <summary>
        /// 物品ID
        /// </summary>
        public TKey ItemId { get; set; }



        /// <summary>
        /// 物品Code
        /// </summary>
        public string ItemCode { get; set; }

    }
}