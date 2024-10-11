using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFireJob.IServices.Dto
{
    public class CronTypeDto
    {
        /// <summary>
        /// 周期性任务类型
        /// </summary>
        public CronStateEnum CronType { get; set; }
        /// <summary>
        /// 执行周期的间隔
        /// </summary>
        public int Interval { get; set; }
        /// <summary>
        /// 第几分钟开始，默认为第一分钟
        /// </summary>
        public int Minute { get; set; } = 1;
        /// <summary>
        /// 第几小时开始，默认从1点开始
        /// </summary>
        public int Hour { get; set; }
        /// <summary>
        /// 几号开始，默认从一号开始
        /// </summary>
        public int Day { get; set; }
        /// <summary>
        /// 星期几开始，默认从星期一点开始
        /// </summary>
        public DayOfWeek Week { get; set; }
        /// <summary>
        /// 几月开始，默认从一月开始
        /// </summary>
        public int Month { get; set; }
    }

    public enum CronStateEnum
    {
        /// <summary>
        /// 周期性为分钟的任务
        /// </summary>
        [Description("周期性为分钟的任务")]
        Minute = 1,
        /// <summary>
        /// 周期性为小时的任务
        /// </summary>
        [Description("周期性为小时的任务")]
        Hour = 2,
        /// <summary>
        /// 周期性为天的任务
        /// </summary>
        [Description("周期性为天的任务")]
        Day = 3,
        /// <summary>
        /// 周期性为周的任务
        /// </summary>
        [Description("周期性为周的任务")]
        Week = 4,
        /// <summary>
        /// 周期性为月的任务
        /// </summary>
        [Description("周期性为月的任务")]
        Month = 5,
        /// <summary>
        /// 周期性为年的任务
        /// </summary>
        [Description("周期性为年的任务")]
        Year = 6

    }
}
