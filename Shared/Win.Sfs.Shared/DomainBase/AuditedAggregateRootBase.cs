using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Win.Sfs.Shared.DomainBase
{
    public abstract class AuditedAggregateRootBase<TKey> : AuditedAggregateRoot<TKey>,IBranch<TKey>, IEnabled, IRemark
        
    {
        protected AuditedAggregateRootBase() { }
        public AuditedAggregateRootBase(TKey id) : base(id) { }

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