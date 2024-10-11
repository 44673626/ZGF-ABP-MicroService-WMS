using System;

namespace Win.Sfs.Shared.DomainBase
{
    public abstract class BranchEntityBase<TKey> : EntityBase<TKey>, IBranch<TKey>
    {
        public BranchEntityBase()
        {
        }

        public BranchEntityBase(TKey id) : base(id)
        {
        }

        public TKey BranchId { get; set; }
    }
}