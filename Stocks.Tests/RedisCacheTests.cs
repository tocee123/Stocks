using AutoFixture;
using NUnit.Framework;
using Stocks.Core.Cache;
using Stocks.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebDownloading.Test
{
    [TestFixture]
    public class RedisCacheTests
    {
        private RedisCachedRepository _target;
        [SetUp]
        public void Setup()
        {
            _target = new RedisCachedRepository();
        }

        [Test]
        public async Task WriteToCache_WritesObjectInJsonFormat()
        {
            var fixture = new Fixture();
            var stockDividends = fixture.Create<IEnumerable<StockDividend>>();
            var key = $"{DateTime.Now.ToString("yyyy-MM-dd")}_{nameof(WriteToCache_WritesObjectInJsonFormat)}";
            await _target.WriteToCacheAsync(key, stockDividends, 60);
            var result = await _target.ReadFromCacheAsync<IEnumerable<StockDividend>>(key);

            Assert.IsNotNull(result);
        }

        [Test]
        public async Task ReadStringFromCache_WhenKeyExists_ReturnsString()
        {
            var key = "123";
            var result = await _target.ReadStringFromCacheAsync(key);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task WriteStocksOfInterestAsync()
        {
            var target = new RedisCachedRepository();
            await target.WriteStocksOfInterestAsync();
        }
    }
}