using Volo.Abp.Domain.Entities.Auditing;

namespace Win.Sfs.Shared.DomainBase
{
    public abstract class CreationAggregateRootBase<TKey> : CreationAuditedAggregateRoot<TKey>, IEnabled, IRemark
    {
        public bool Enabled { get; set; } = true;
        public string Remark { get; set; }
    }
}