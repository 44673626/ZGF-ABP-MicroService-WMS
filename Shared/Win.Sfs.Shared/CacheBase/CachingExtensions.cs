using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Caching;

namespace Win.Sfs.Shared.CacheBase
{
    public static class CachingExtensions
    {
        private static DistributedCacheEntryOptions CreateDistributedCacheEntryOptions<TCacheItem>(int minutes)
        {
            var options = new DistributedCacheEntryOptions();
            if (minutes != CacheStrategyConst.NEVER)
            {
                options.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(minutes);
            }

            return options;
        }

        /// <summary>
        /// 获取或添加缓存
        /// </summary>
        /// <typeparam name="TCacheItem"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="factory"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static async Task<TCacheItem> GetOrAddAsync<TCacheItem>(
            this IDistributedCache<TCacheItem> cache, string key, Func<Task<TCacheItem>> factory, int minutes) where TCacheItem : class
        {
            TCacheItem cacheItem;

            var result = await cache.GetAsync(key);
            if (result == null)
            {
                cacheItem = await factory.Invoke();
                await cache.SetAsync(key, cacheItem, minutes);
            }
            else
            {
                cacheItem = result;
            }

            return cacheItem;
        }

        // public static async Task<TCacheItem> GetOrAddAsync<TCacheItem>(this IDistributedCache cache, string key, TCacheItem cacheItem, int minutes)
        // {
        //
        //     var result = await cache.GetStringAsync(GetKey(typeof(TCacheItem), key));
        //     if (string.IsNullOrEmpty(result))
        //     {
        //         var options = new DistributedCacheEntryOptions();
        //         if (minutes != CacheStrategyConst.NEVER)
        //         {
        //             options.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(minutes);
        //         }
        //         await cache.SetStringAsync(GetKey(typeof(TCacheItem), key), cacheItem.ToJson(), options);
        //     }
        //     else
        //     {
        //         cacheItem = result.FromJson<TCacheItem>();
        //     }
        //     return cacheItem;
        // }

        public static async Task<IEnumerable<TCacheItem>> GetManyAsync<TCacheItem>(
            this IDistributedCache<TCacheItem> cache, IEnumerable<string> keys) where TCacheItem : class
        {
            var cacheItems = await cache.GetManyAsync(keys, null, true);
            return cacheItems.Select(p => p.Value);
        }

        public static async Task SetAsync<TCacheItem>(
            this IDistributedCache<TCacheItem> cache, string key, TCacheItem cacheItem, int minutes) where TCacheItem : class
        {
            var options = CreateDistributedCacheEntryOptions<TCacheItem>(minutes);
            await cache.SetAsync(key, cacheItem, options, null, true);
        }

        public static async Task SetManyAsync<TCacheItem>(
            this IDistributedCache<TCacheItem> cache, IEnumerable<KeyValuePair<string, TCacheItem>> cacheItems, int minutes) where TCacheItem : class
        {
            var options = CreateDistributedCacheEntryOptions<TCacheItem>(minutes);
            await cache.SetManyAsync(cacheItems, options);
        }

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