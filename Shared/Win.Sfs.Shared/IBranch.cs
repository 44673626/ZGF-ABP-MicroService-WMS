using System;

namespace Win.Sfs.Shared
{
    public interface IBranch<TKey>
    {
        TKey BranchId { get; set; }
    }
}