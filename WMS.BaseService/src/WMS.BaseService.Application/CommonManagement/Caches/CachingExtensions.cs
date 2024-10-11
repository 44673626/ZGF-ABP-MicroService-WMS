using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Caching;

namespace WMS.BaseService.CommonManagement.Caches
{
    /// <summary>
    /// 自定义分布式缓存扩展方法
    /// </summary>
    public static class CachingExtensions
    {
        /// <summary>
        /// 缓存配置，使用绝对还是滑动过期，不使用策略就默认为长期保存
        /// </summary>
        /// <typeparam name="TCacheItem"></typeparam>
        /// <param name="minutes"></param>
        /// <returns></returns>
        private static DistributedCacheEntryOptions CreateDistributedCacheEntryOptions<TCacheItem>(int minutes)
        {
            var options = new DistributedCacheEntryOptions();
            if (minutes != AbpCacheConst.Never)
            {
                options.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(minutes);//设置绝对过期时间
            }

            return options;
        }

        /// <summary>
        /// 获取或添加缓存
        /// </summary>
        /// <typeparam name="TCacheItem">缓存类</typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="factory"></param>
        /// <param name="minutes">缓存过期时间</param>
        /// <returns></returns>
        public static async Task<TCacheItem> GetOrAddAsync<TCacheItem>(
            this IDistributedCache<TCacheItem> cache, string key, Func<Task<TCacheItem>> factory, int minutes)
            where TCacheItem : class
        {
            TCacheItem cacheItem;

            var result = await cache.GetAsync(key);
            if (result == null)
            {
                cacheItem = await factory.Invoke();
                await cache.SetValueAsync(key, cacheItem, minutes);
            }
            else
            {
                cacheItem = result;
            }
            return cacheItem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="keys"></param>
        /// <typeparam name="TCacheItem"></typeparam>
        /// <returns></returns>
        public static async Task<IEnumerable<TCacheItem>> GetManyAsync<TCacheItem>(
            this IDistributedCache<TCacheItem> cache, IEnumerable<string> keys) where TCacheItem : class
        {
            var cacheItems = await cache.GetManyAsync(keys, null, true);
            return cacheItems.Select(p => p.Value);
        }

        /// <summary>
        ///SetAsync的第四个参数： considerUow, 默认为false. 如果将其设置为true,
        ///则你对缓存所做的更改不会应用于真正的缓存存储, 而是与当前的工作单元关联.
        ///你将获得在同一工作单元中设置的缓存值, 但仅当前工作单元成功时更改才会生效.
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="cacheItem"></param>
        /// <param name="minutes"></param>
        /// <typeparam name="TCacheItem"></typeparam>
        public static async Task SetValueAsync<TCacheItem>(
            this IDistributedCache<TCacheItem> cache, string key, TCacheItem cacheItem, int minutes) where TCacheItem : class
        {
            var options = CreateDistributedCacheEntryOptions<TCacheItem>(minutes);
            await cache.SetAsync(key, cacheItem, options, null, true);
        }

        /// <summary>
        /// 批量设置缓存
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="cacheItems"></param>
        /// <param name="minutes"></param>
        /// <typeparam name="TCacheItem"></typeparam>
        public static async Task SetManyAsync<TCacheItem>(
            this IDistributedCache<TCacheItem> cache, IEnumerable<KeyValuePair<string, TCacheItem>> cacheItems, int minutes) where TCacheItem : class
        {
            var options = CreateDistributedCacheEntryOptions<TCacheItem>(minutes);
            await cache.SetManyAsync(cacheItems, options);
        }


        /// <summary>
        /// 删除缓存
        /// </summary> 
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <typeparam name="TCacheItem"></typeparam>
        public static async Task DeleteAsync<TCacheItem>(
            this IDistributedCache<TCacheItem> cache, string key)
            where TCacheItem : class
        {
            var result = await cache.GetAsync(key);
            if (result != null)
            {
                await cache.RemoveAsync(key);
            }
        }


        private static string GetKey(Type type, string key)
        {
            return $"{type.Name}:{key}";
        }

    }
}
