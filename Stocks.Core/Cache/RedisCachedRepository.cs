using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Stocks.Core.Cache
{
    public class RedisCachedRepository : ICachedRepository
    {
        const string _redisConnectionString = "stockredis.redis.cache.windows.net:6380,password=jxQt8QqLPu7ezjjBjANAhmVx2v3hftB5YhFwZXqhOHk=,ssl=True,abortConnect=False";
        public T ReadFromCache<T>(string key)
        {
            using var cm = ConnectionMultiplexer.Connect(_redisConnectionString);
            var db = cm.GetDatabase();
            string value = db.StringGet(key);
            return value is null ? default
                : JsonConvert.DeserializeObject<T>(value);
        }

        public async Task<T> ReadFromCacheAsync<T>(string key)
        {
            var value = await ReadStringFromCacheAsync(key);
            return value is null ? default
                : JsonConvert.DeserializeObject<T>(value);
        }

        public async Task<string> ReadStringFromCacheAsync(string key)
        {
            using var cm = ConnectionMultiplexer.Connect(_redisConnectionString);
            var db = cm.GetDatabase();
            return await db.StringGetAsync(key);
        }

        public async Task WriteToCacheAsync<T>(string key, T value, int? expiration = null)
        {
            var expiry = GetExpiration(expiration);
            var convertedValue = JsonConvert.SerializeObject(value);
            using var cm = ConnectionMultiplexer.Connect(_redisConnectionString);
            var db = cm.GetDatabase();
            await db.StringSetAsync(key, convertedValue, expiry);
        }

        private TimeSpan? GetExpiration(int? expiration)
        => expiration == -1 ? null
            : TimeSpan.FromSeconds(expiration ?? 3600);
    }
}
