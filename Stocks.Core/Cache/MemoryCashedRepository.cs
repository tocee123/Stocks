using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Stocks.Core.Enums;
using System.Threading;

namespace Stocks.Core.Cache;
public class MemoryCashedRepository : ICachedRepository
{
    private readonly IMemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());
    private static CancellationTokenSource _resetCacheToken = new();

    public async Task Clear()
    {
        await Task.Run(() =>
        {
            if (_resetCacheToken != null && !_resetCacheToken.IsCancellationRequested && _resetCacheToken.Token.CanBeCanceled)
            {
                _resetCacheToken.Cancel();
                _resetCacheToken.Dispose();
            }

            _resetCacheToken = new CancellationTokenSource();
        });
    }

    public T Get<T>(string key)
    {
        return _memoryCache.Get<T>(key);
    }

    public async Task<T> GetAsync<T>(string key)
    {
        return await Task.Run(() => _memoryCache.Get<T>(key));
    }

    public async Task<string> GetStringAsync(string key)
    {
        return await Task.Run(() => _memoryCache.Get<string>(key));
    }

    public async Task SetAsync<T>(string key, T value, CacheDuration cacheDuration = CacheDuration.OneHour)
    {
        var cacheEntryOption = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = cacheDuration.GetExpiration()
        };
        cacheEntryOption.AddExpirationToken(new CancellationChangeToken(_resetCacheToken.Token));
        await Task.Run(() => _memoryCache.Set(key, value, cacheEntryOption));
    }
}