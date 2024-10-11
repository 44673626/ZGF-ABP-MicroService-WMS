// 闻荫智慧工厂管理套件
//  Copyright (c) 闻荫科技 www.ccwin-in.com

using Volo.Abp.Domain.Entities.Auditing;

namespace Win.Sfs.Shared.DomainBase
{
    public abstract class AggregateRootBase<TKey> : FullAuditedAggregateRoot<TKey>, IEnabled, IRemark
        // ,IMultiTenant
    {
        public AggregateRootBase()
        {
        }

        public AggregateRootBase(TKey id) : base(id)
        {
        }

        public bool Enabled { get; set; } = true;

        public string Remark { get; set; }

        // public Guid? TenantId { get; }
    }
}