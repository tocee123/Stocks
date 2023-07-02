using System.Net.Http;
using System.Net;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using System.IO;

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

        [Test]
        public async Task TaskTest()
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

            Console.WriteLine(content);
        }

        //TODO fix the test
        [Test]
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

        //TODO
        [Ignore("something is not working")]
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

        [Test]
        public void FillProperties_tests()
        {
            var ticker = "qyld";
            var fileContent = File.ReadAllText($@"..\..\..\Files\{ticker}.html");
            Assert.IsNotNull(fileContent);
            var stock = new StockDividend();
            StockDividendHistoryLoader.FillProperties(stock, ticker, fileContent);
        }
    }
}