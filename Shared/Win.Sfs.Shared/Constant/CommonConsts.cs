using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win.Sfs.Shared.Constant
{
    public static class CommonConsts
    {
        public const int MaxNumeralLength = 16;

        public const int MaxCodeLength = 36;

        public const int MaxTypeLength = 32;

        public const int MaxNameLength = 64;

        public const int MaxFullNameLength = 1024;

        public const int MaxDescriptionLength = 2048;
        public static double CacheAbsoluteExpirationMinutes { get; set; } = 10;
        public static int MaxValueLength { get; set; } = 1024;

        public static Guid TestGuid = new Guid("7CD45A6D-762A-6F0F-B9ED-39F91EEA93D1");

    }
}
