using System;
using Microsoft.AspNetCore.Http;
using Win.Utils;

namespace Win.Sfs.Shared.CurrentBranch
{
    public class BranchManager: IBranchManager 
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        protected BranchManager() { }

        public BranchManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public virtual Guid GetId()
        {
            var context = _httpContextAccessor.HttpContext;

            var strBranchId = context?.Request.Headers[BranchHeaderConsts.HeaderName];
            if (string.IsNullOrEmpty(strBranchId))
                return Guid.NewGuid();
            var branchId = Guid.Parse(strBranchId);
            return branchId;
        }

        // public virtual T GetId<T>()
        // {
        //     var context = _httpContextAccessor.HttpContext;
        //
        //     var strBranchId = context?.Request.Headers[BranchHeaderConsts.HeaderName];
        //     if (string.IsNullOrEmpty(strBranchId))
        //         return default;
        //     var t = strBranchId.ChangeType<T>();
        //     return t;
        // }
    }
}