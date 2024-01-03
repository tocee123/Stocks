using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using Stocks.Core.Enums;

namespace Stocks.Core.Cache;
public class RedisCachedRepository : ICachedRepository
{
    readonly ConfigurationOptions _redisConnectionString;

    public RedisCachedRepository(IOptionsMonitor<DbAccess> settings)
    {
        _redisConnectionString = ConfigurationOptions.Parse(settings.Get(DbAccess.Redis).ConnectionString);
        _redisConnectionString.ConnectRetry = 5;
        _redisConnectionString.AllowAdmin = true;
    }

    public async Task Clear()
    {
        await Task.Run(() =>
        {
            using var cm = ConnectionMultiplexer.Connect(_redisConnectionString);
            var endpoints = cm.GetEndPoints(true);
            foreach (var endpoint in endpoints)
            {
                var server = cm.GetServer(endpoint);
                server.FlushAllDatabases();
            }
        });
    }

    public T Get<T>(string key)
    {
        using var cm = ConnectionMultiplexer.Connect(_redisConnectionString);
        var db = cm.GetDatabase();
        string value = db.StringGet(key);
        return value is null ? default
            : JsonConvert.DeserializeObject<T>(value);
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var value = await GetStringAsync(key);
        return value is null ? default
            : JsonConvert.DeserializeObject<T>(value);
    }

    public async Task<string> GetStringAsync(string key)
    {
        using var cm = ConnectionMultiplexer.Connect(_redisConnectionString);
        var db = cm.GetDatabase();
        return await db.StringGetAsync(key);
    }

    public async Task SetAsync<T>(string key, T value, CacheDuration cacheDuration = CacheDuration.OneHour)
    {
        var expiry = cacheDuration.GetExpiration();
        var convertedValue = JsonConvert.SerializeObject(value);
        using var cm = ConnectionMultiplexer.Connect(_redisConnectionString);
        var db = cm.GetDatabase();
        await db.StringSetAsync(key, convertedValue, expiry);
    }
}