using Microsoft.Extensions.Configuration;
using NSubstitute;
using Stocks.Web.Pages;
using System.Diagnostics;

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
            _target = new StocksRepository(new StocksLoader(new StockDividendHistoryLoader()), new StocksOfInterestRespository());
        }

        [Test]
        public async Task GetStocks_ReturnsNotEmptyList()
        {
            var sw = Stopwatch.StartNew();
            var result = await _target.GetStocksAsync();
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }
    }
}