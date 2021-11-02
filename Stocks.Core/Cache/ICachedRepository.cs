using Stocks.Core.Enums;
using System.Threading.Tasks;

namespace Stocks.Core.Cache
{
    public interface ICachedRepository
    {
        Task<T> ReadFromCacheAsync<T>(string key);
        Task<string> ReadStringFromCacheAsync(string key);
        Task WriteToCacheAsync<T>(string key, T value, CacheDuration cacheDuration = CacheDuration.OneHour);
        T ReadFromCache<T>(string key);
    }
}