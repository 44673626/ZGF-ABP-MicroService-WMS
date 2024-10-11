namespace Win.Sfs.Shared.DomainBase
{
    public abstract class CreationAuditedDocumentDetailBase<TKey>
        : CreationAuditedEntityBase<TKey>
            , IDocumentNumber
            , IItem<TKey>
    {
        protected CreationAuditedDocumentDetailBase() { }

        public CreationAuditedDocumentDetailBase(TKey id) : base(id) { }

        /// <summary>
        /// 单据流水号
        /// </summary>
        public string DocumentNumber { get; set; }


        /// <summary>
        /// 物品ID
        /// </summary>
        public TKey ItemId { get; set; }

        /// <summary>
        /// 物品代码
        /// </summary>
        public string ItemCode { get; set; }
    }
}