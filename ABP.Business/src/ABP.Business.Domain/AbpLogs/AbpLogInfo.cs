using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ABP.Business.Logs
{
    //自定义日志表
    public class AbpLogInfo :Entity<Guid>
    {
        public AbpLogInfo() { }
        public void SetValue(
          Guid Id)
        {
            this.Id = Id;
        }
        /// <summary>
        /// 日志信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 日志消息模板
        /// </summary>
        public string MessageTemplate { get; set; }

        /// <summary>
        /// 日志级别
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// 时间戳，记录日志的时间
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public string Exception { get; set; }

        /// <summary>
        /// 其他属性信息，通常以 JSON 格式存储
        /// </summary>
        public string Properties { get; set; }



    }
}
