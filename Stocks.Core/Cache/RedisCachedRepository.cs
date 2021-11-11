﻿using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using Stocks.Core.Enums;
using System;
using System.Threading.Tasks;

namespace Stocks.Core.Cache
{
    public class RedisCachedRepository : ICachedRepository
    {
        readonly string _redisConnectionString;

        public RedisCachedRepository(IConfiguration configuration)
        {
            _redisConnectionString = configuration.GetConnectionString("Redis");
        }

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

        public async Task WriteToCacheAsync<T>(string key, T value, CacheDuration cacheDuration = CacheDuration.OneHour)
        {
            var expiry = GetExpiration(cacheDuration);
            var convertedValue = JsonConvert.SerializeObject(value);
            using var cm = ConnectionMultiplexer.Connect(_redisConnectionString);
            var db = cm.GetDatabase();
            await db.StringSetAsync(key, convertedValue, expiry);
        }

        private static TimeSpan? GetExpiration(CacheDuration cacheDuration)
        => cacheDuration switch
        {
            CacheDuration.Default or CacheDuration.OneHour => CreateTimespanFromSeconds(3600),
            CacheDuration.OneMinute => CreateTimespanFromSeconds(60),
            CacheDuration.Unlimited => null,
            _ => throw new NotImplementedException()
        };

        private static TimeSpan CreateTimespanFromSeconds(int seconds)
            => TimeSpan.FromSeconds(seconds);
    }
}