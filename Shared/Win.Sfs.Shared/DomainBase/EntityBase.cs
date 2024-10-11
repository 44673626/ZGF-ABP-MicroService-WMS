using Volo.Abp.Domain.Entities.Auditing;

namespace Win.Sfs.Shared.DomainBase
{
    public abstract class EntityBase<TKey> : FullAuditedEntity<TKey>, IEnabled, IRemark
    {
        public EntityBase()
        {
        }

        public EntityBase(TKey id) : base(id)
        {
        }

        public bool Enabled { get; set; } = true;

        public string Remark { get; set; }
    }
}