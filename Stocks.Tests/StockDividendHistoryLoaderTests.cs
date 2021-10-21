using NUnit.Framework;
using Stocks.Core;
using System.Linq;
using System.Threading.Tasks;

namespace WebDownloading.Test
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

        [Test]
        public async Task DownloadStockHistoryAsync_WhenCorrectStockShortNameIsGiven_ReturnsNotemptyClass()
        {
            var stockShortName = "MPLX";
            var result = await _target.DownloadStockHistoryAsync(stockShortName);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsCorrectlyDownloaded);
            Assert.AreEqual(stockShortName, result.ShortName);
            Assert.IsTrue(result.DividendHistories.Any());
            Assert.IsNotNull(result.LatestDividendHistory);
        }

        [Test]
        public async Task DownloadStockHistoryAsync_WhenIncorrectStockShortNameIsGiven_IsCorrectlyDownloadedFalse()
        {
            var stockShortName = "123asd";
            var result = await _target.DownloadStockHistoryAsync(stockShortName);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsCorrectlyDownloaded);
        }
    }
}