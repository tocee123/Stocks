using Stocks.Core.Enums;

namespace Stocks.Core.Cache
{
    public interface ICachedRepository
    {
        Task<T> GetAsync<T>(string key);
        Task<string> GetStringAsync(string key);
        Task SetAsync<T>(string key, T value, CacheDuration cacheDuration = CacheDuration.OneHour);
        T Get<T>(string key);
        Task Clear();
    }
}