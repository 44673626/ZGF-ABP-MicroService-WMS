using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Win.Sfs.Shared.DomainBase
{
    public abstract class FullAuditedEntityBase<TKey> : FullAuditedEntity<TKey>,IBranch<TKey>, IEnabled, IRemark
    {
        protected FullAuditedEntityBase() { }
        public FullAuditedEntityBase(TKey id) : base(id) { }

        /// <summary>
        /// 分支ID
        /// </summary>
        public TKey BranchId { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}