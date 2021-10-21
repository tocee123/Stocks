using NUnit.Framework;
using Stocks.Core;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace WebDownloading.Test
{
    [TestFixture]
    public class StocksRepositoryTests
    {
        private StocksRepository _target;
        [SetUp]
        public void Setup()
        {
            _target = new StocksRepository(new StocksLoader(new StockDividendHistoryLoader()), new RedisCache());
        }

        [Test]
        public async Task GetStocks_ReturnsNotEmptyList()
        {
            var sw = Stopwatch.StartNew();
            var result = await _target.GetStocks();
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }
    }
}