// 闻荫智慧工厂管理套件
//  Copyright (c) 闻荫科技 www.ccwin-in.com

using System;

namespace Win.Sfs.Shared.DomainBase
{
    public abstract class BranchAggregateRootBase<TKey> : AggregateRootBase<TKey>, IBranch<TKey>
    {
        public BranchAggregateRootBase()
        {
        }

        public BranchAggregateRootBase(TKey id) : base(id)
        {
        }

        public TKey BranchId { get; set; }
    }
}