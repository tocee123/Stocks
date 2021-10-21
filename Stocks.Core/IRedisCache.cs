using System.Threading.Tasks;

namespace Stocks.Core
{
    public interface IRedisCache
    {
        Task<T> ReadFromCacheAsync<T>(string key);
        Task<string> ReadStringFromCacheAsync(string key);
        Task WriteToCacheAsync<T>(string key, T value, int? expiration = null);
        T ReadFromCache<T>(string key);
    }
}