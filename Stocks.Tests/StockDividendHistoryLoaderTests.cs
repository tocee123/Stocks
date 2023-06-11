using NUnit.Framework;
using Stocks.Core.Loaders;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Stocks.Test
{
    [TestFixture]
    public class StockDividendHistoryLoaderTests
    {
        private IStockDividendHistoryLoader _target;

        [SetUp]
        public void Setup()
        {
            _target = new StockDividendHistoryLoader();
        }

        //TODO fix the test
        //[Test]
        public async Task DownloadStockHistoryAsync_WhenCorrectStockTickerIsGiven_ReturnsNotemptyClass()
        {
            var ticker = "MPLX";
            var result = await _target.DownloadStockHistoryAsync(ticker);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsCorrectlyDownloaded);
            Assert.AreEqual(ticker, result.Ticker);
            Assert.IsTrue(result.DividendHistories.Any());
            Assert.IsNotNull(result.LatestDividendHistory);
        }

        [TestCase("TAEF")]
        [TestCase("123asd")]
        public async Task DownloadStockHistoryAsync_WhenIncorrectStockTickerIsGiven_IsCorrectlyDownloadedFalse(string ticker)
        {
            var result = await _target.DownloadStockHistoryAsync(ticker);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsCorrectlyDownloaded);
        }

        //TODO
        //[TestCase("abc", false)]
        //[TestCase("www.google.com", true)]
        //[TestCase("http://google.com", true)]
        public async Task DoesPageExist_WhenPageDoesNotExist_ReturnsFalse(string url, bool expected)
        {
            var (pageExists, resultUrl) = await StockDividendHistoryLoader.DoesPageExist(url);
            Assert.AreEqual(expected, pageExists);
            Console.WriteLine(resultUrl);
        }

        [TestCase("abc", false)]
        [TestCase("www.google.com", true)]
        [TestCase("http://google.com", true)]
        public void IsCorrectUrl_WhenUrlIsGiven_ReturnsResultAccordingly(string url, bool expected)
        {
            var result = StockDividendHistoryLoader.IsCorrectUrl(url, out var reusltUri);
            Assert.AreEqual(expected, result);
        }
    }
}