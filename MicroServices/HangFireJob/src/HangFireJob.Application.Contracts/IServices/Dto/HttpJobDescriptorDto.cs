using HangFireJob.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace HangFireJob.IServices.Dto
{
    public class HttpJobDescriptorDto : EntityDto<Guid>
    {
        /// <summary>
        /// 租户
        /// </summary>
        public Guid? TenantId { get; set; }
        /// <summary>
        /// WebApi请求地址，格式：http://www.xxx.com/
        /// </summary>
        public string HttpUrl { get; set; }
        /// <summary>
        /// 任务ID（用于关联HangFire数据库中的Job中的Id）
        /// </summary>
        public int JobId { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string JobName { get; set; }
        /// <summary>
        /// job的唯一标识 如果没有设置值的话就等同于JobName
        /// </summary>
        //public string RecurringJobIdentifier { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        public string JobType { get; set; }
        /// <summary>
        /// Api访问类型（Get, Post, Put, Delete）
        /// </summary>
        public string HttpMethod { get; set; }
        /// <summary>
        /// 传参参数（post/put/delete）
        /// </summary>
        public object JobParameter { get; set; }
        /// <summary>
        /// cron表达式
        /// </summary>
        public string Cron { get; set; }
        /// <summary>
        /// 延迟加载，单位:分钟
        /// </summary>
        public double DelayInMinute { get; set; }

        /// <summary>
        /// 延时加载任务设置的时间，单位：秒,0代表 立即执行 >0 代表延迟秒数
        /// </summary>
        public int TimeSpanFromSeconds { get; set; } = 0;
        /// <summary>
        /// 任务描述
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 执行状态
        /// </summary>
        public string StateName { get; set; }

        /// <summary>
        /// QueueName的名称 如果不配置就用默认的 DEFAULT
        /// </summary>
        //public string QueueName { get; set; }

        /// <summary>
        /// 针对定时任务：最近一次运行状态
        /// </summary>
        public string LastJobState { get; set; }

        /// <summary>
        /// 针对定时任务：最近一次运行时间
        /// </summary>
        public DateTime? LastExecution { get; set; }

        /// <summary>
        /// Get类型参数
        /// </summary>
        public List<GetParamModel> GetParams { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreationTime { get; set; }
    }
}
