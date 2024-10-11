﻿using ABP.Business.CommonManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;

namespace ABP.Business.Bases
{
    public class AbpAuthEfCoreRepositoryBase<TDbContext, TEntity, TKey>
    : AbpEfCoreRepositoryBase<TDbContext, TEntity, TKey>
    , IAbpRepositoryBase<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TDbContext : IEfCoreDbContext
    {
        public AbpAuthEfCoreRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
