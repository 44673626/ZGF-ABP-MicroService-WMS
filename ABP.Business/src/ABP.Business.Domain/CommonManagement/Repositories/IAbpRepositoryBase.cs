using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace ABP.Business.CommonManagement.Repositories
{
    public interface IAbpRepositoryBase<TEntity, TKey> : IRepository<TEntity, TKey>, IAbpReadonlyRepositoryBase<TEntity, TKey>
     where TEntity : class, IEntity<TKey>
    {

    }
}
