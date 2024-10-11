using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using Volo.Abp;
using System.Diagnostics.CodeAnalysis;

namespace WMS.Business.Samples.DataDictionarys
{
    /// <summary>
    /// 用于测试分布式缓存Redis
    /// </summary>
    public class DataDictionary : AuditedAggregateRoot<Guid>, ISoftDelete, IMultiTenant
    {
        /// <summary>
        /// 默认无参数构造函数，必须有，不然依赖注入失败
        /// </summary>
        public DataDictionary() { }
        public Guid? TenantId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public DataDictionary(Guid id, [NotNull] string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}
