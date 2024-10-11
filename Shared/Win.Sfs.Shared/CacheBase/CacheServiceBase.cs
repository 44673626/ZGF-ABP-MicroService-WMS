using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace Win.Sfs.Shared.CacheBase
{
    public abstract class CacheServiceBase<TCacheItem> : ITransientDependency where TCacheItem : Entity
    {
        protected IDistributedCache<TCacheItem> Cache { get; }

        protected CacheServiceBase() { }

        protected CacheServiceBase(IDistributedCache<TCacheItem> cache)
        {
            Cache = cache;
        }

        public virtual async Task<string> GetPropertyValueAsync(Func<Task<TCacheItem>> factory, string propertyName)
        {
            return await GetPropertyValueAsync(factory, new List<string> { propertyName });
        }

        public virtual async Task<string> GetPropertyValueAsync(Func<Task<TCacheItem>> factory, IEnumerable<string> propertyNames, char separator = ',')
        {
            try
            {
                var entity =  await factory.Invoke();
                var sb = new StringBuilder();
                foreach (var propertyName in propertyNames)
                {
                    var entityType = entity.GetType();
                    var property = entityType.GetProperty(propertyName);
                    if (property == null)
                    {
                        throw new AbpException($"can't find Property:{propertyName} from Entity:{entityType.Name}");
                    }

                    var propertyValue = property.GetValue(entity, null);

                    sb.Append(propertyValue + separator.ToString());

                }
                return  sb.ToString().TrimEnd(separator);
            }
            catch (EntityNotFoundException)
            {
                return string.Empty;
            }
        }

    }

}