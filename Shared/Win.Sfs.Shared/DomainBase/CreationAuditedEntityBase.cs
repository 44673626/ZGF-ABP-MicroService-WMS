using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Win.Sfs.Shared.DomainBase
{
    public abstract class CreationAuditedEntityBase<TKey> : CreationAuditedEntity<TKey>, IBranch<TKey>, IRemark
    {
        protected CreationAuditedEntityBase() { }
        public CreationAuditedEntityBase(TKey id) : base(id) { }

        /// <summary>
        /// 分支ID
        /// </summary>
        public TKey BranchId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}