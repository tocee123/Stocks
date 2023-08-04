using System.Net.Http;
using System.Net;
using System.IO;
using Microsoft.Extensions.Logging.Abstractions;

namespace Stocks.Test.Stocks.Core.Loaders
{
    [TestFixture]
    public class StockDividendHistoryLoaderShould
    {
        private IStockDividendHistoryLoader _target;

        [SetUp]
        public void Setup()
        {
            _target = new StockDividendHistoryLoader(NullLogger<StockDividendHistoryLoader>.Instance);
        }

        [Test]
        public async Task ReturnWebPageContent()
        {
            string url = "https://dividendhistory.org/payout/QYLD/";
            var handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            using var client = new HttpClient(handler);
            client.Timeout = TimeSpan.FromSeconds(2);
            client.DefaultRequestHeaders.Add("Accept", "*/*");
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");

            var content = await client.GetStringAsync(url);
            content.Should().NotBeNullOrEmpty();
            Console.WriteLine(content);
        }

        [Test]
        public async Task ReturnNotEmptyInstanceWhenCorrectStockTickerIsGiven()
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
        public async Task SetIsCorrectlyDownloadedToFalseWhenIncorrectStockTickerIsGiven(string ticker)
        {
            var result = await _target.DownloadStockHistoryAsync(ticker);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsCorrectlyDownloaded);
        }

        [TestCase("abc", false)]
        [TestCase("www.google.com", true)]
        [TestCase("http://google.com", true)]
        public void CheckIfGivenUrlIsValid(string url, bool expected)
        {
            var result = StockDividendHistoryLoader.IsCorrectUrl(url, out var reusltUri);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ReadStocksFromFile()
        {
            var ticker = "qyld";
            var fileContent = File.ReadAllText($@"..\..\..\Files\{ticker}.html");
            Assert.IsNotNull(fileContent);
            var stock = new StockDividend();
            StockDividendHistoryLoader.FillProperties(stock, ticker, fileContent);
            Assert.AreNotEqual(0.0, stock.Price);
        }
    }
}