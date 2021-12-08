using Microsoft.Extensions.Caching.Memory;
using Stocks.Core.Enums;
using System.Threading.Tasks;

namespace Stocks.Core.Cache
{
    public class MemoryCashedRepository : ICachedRepository
    {
        private readonly IMemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());

        public T ReadFromCache<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public Task<T> ReadFromCacheAsync<T>(string key)
        {
            return Task.Run(() => _memoryCache.Get<T>(key));
        }

        public Task<string> ReadStringFromCacheAsync(string key)
        {
            return Task.Run(() => _memoryCache.Get<string>(key));
        }

        public async Task WriteToCacheAsync<T>(string key, T value, CacheDuration cacheDuration = CacheDuration.OneHour)
        {
            var cacheEntryOption = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = cacheDuration.GetExpiration()
            };

            await Task.Run(() => _memoryCache.Set(key, value, cacheEntryOption));
        }
    }
}
