using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using Volo.Abp;
using System.ComponentModel.DataAnnotations.Schema;

namespace HangFireJob.Settings
{
    /// <summary>
    /// 任务调度表
    /// </summary>
    public class HttpJobDescriptor : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// 租户
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string JobName { get; set; }
        /// <summary>
        /// 任务类型(单次任务、延时任务和定时任务)
        /// </summary>
        public string JobType { get; set; }
        /// <summary>
        /// WebApi请求地址，格式：http://www.xxx.com/
        /// </summary>
        public string HttpUrl { get; set; }
        /// <summary>
        /// 请求类型（Get, Post, Put, Delete）
        /// </summary>
        public string HttpMethod { get; set; }
        /// <summary>
        /// 传参参数（post/put/delete）
        /// </summary>
        public string JobParameter { get; set; }
        /// <summary>
        /// cron表达式
        /// </summary>
        public string Cron { get; set; }
        /// <summary>
        /// 延迟加载，单位:分钟
        /// </summary>
        public double DelayInMinute { get; set; }

        /// <summary>
        /// 延时加载任务设置的时间，单位：秒
        /// </summary>
        public int TimeSpanFromSeconds { get; set; }
        /// <summary>
        /// 执行状态
        /// </summary>
        public string StateName { get; set; }
        /// <summary>
        /// 任务ID（用于关联HangFire数据库中的Job中的Id）
        /// </summary>
        public int JobId { get; set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        public string Remark { get; set; }

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
        [NotMapped]
        virtual public List<GetParamModel> GetParams { get; set; }

        /// <summary>
        /// 传递token信息认证地址（必填）
        /// </summary>
        public string AuthUrl { get; set; }
        /// <summary>
        /// 租户名称（可空）
        /// </summary>
        public string TenantName { get; set; }

    }

    /// <summary>
    /// Get参数的结构
    /// </summary>
    public class GetParamModel
    {
        /// <summary>
        /// key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// value值.
        /// </summary>
        public string Value { get; set; }
    }
}
