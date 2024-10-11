using System;

namespace Win.Sfs.Shared.CurrentBranch
{
    public static class A
    {
        public static T ChangeType<T>(this object obj)
        {
            return (T)Convert.ChangeType(obj, typeof(T));
        }
    }
}