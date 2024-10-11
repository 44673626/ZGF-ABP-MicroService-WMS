using System;
using Volo.Abp.DependencyInjection;

namespace Win.Sfs.Shared.CurrentBranch
{
    public interface IBranchManager :ITransientDependency
    {
        Guid GetId();
    }
}