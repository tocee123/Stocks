using Microsoft.Extensions.Configuration;
using NSubstitute;
using NUnit.Framework;
using Stocks.Core;
using Stocks.Core.Cache;
using Stocks.Web.Pages;
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
            var configurationSub = Substitute.For<IConfiguration>();
            _target = new StocksRepository(new StocksLoader(new StockDividendHistoryLoader()), new CachedRepositoryManager(new RedisCachedRepository(configurationSub)));
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

        [Test]
        public async Task GetStocks_()
        {
            var result = await _target.GetStocks();
            Console.WriteLine(result.Count());
            var groupped = result.GroupBy(s => s.LatestDividendHistory.WhenToBuy).Select(s => s.OrderByDescending(s1 => s1.DividendToPrice).First());
            result = result.Where(s => s.LatestDividendHistory.WhenToBuy == DateTime.Parse("2021-10-27")).OrderByDescending(s => s.DividendToPrice);
            foreach (var s in groupped)
            {
                Console.WriteLine($"{s.LatestDividendHistory.WhenToBuy}-> {s.Ticker}, {s.DividendToPrice.ToPercentageDisplay()}");
            }
        }
    }
}