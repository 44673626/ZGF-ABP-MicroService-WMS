using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.BaseService.CommonManagement.Caches
{
    /// <summary>
    /// 用于设置缓存时间
    /// </summary>
    public class AbpCacheConst
    {
        /// <summary>
        /// 
        /// </summary>
        public const int SeveralMinutes = 5;
        /// <summary>
        /// 
        /// </summary>
        public const int SeveralHours = 5 * 60;
        /// <summary>
        /// 
        /// </summary>
        public const int SeveralDays = 5 * 60 * 24;

        /// <summary>
        /// 
        /// </summary>
        public const int Never = 0;
    }
}
