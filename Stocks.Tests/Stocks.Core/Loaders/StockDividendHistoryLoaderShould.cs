using Microsoft.Extensions.Logging.Abstractions;
using System.IO;
using System.Net;
using System.Net.Http;

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

            result.Should().NotBeNull();
            result.IsCorrectlyDownloaded.Should().BeTrue();
            result.Ticker.Should().Be(ticker);
            result.DividendHistories.Any().Should().BeTrue();
            result.LatestDividendHistory.Should().NotBeNull();
        }

        [TestCase("TAEF")]
        [TestCase("123asd")]
        public async Task SetIsCorrectlyDownloadedToFalseWhenIncorrectStockTickerIsGiven(string ticker)
        {
            var result = await _target.DownloadStockHistoryAsync(ticker);

            result.Should().NotBeNull();
            result.IsCorrectlyDownloaded.Should().BeFalse();
        }

        [TestCase("abc", false)]
        [TestCase("www.google.com", true)]
        [TestCase("http://google.com", true)]
        public void CheckIfGivenUrlIsValid(string url, bool expected)
        {
            var result = StockDividendHistoryLoader.IsCorrectUrl(url, out var reusltUri);
            result.Should().Be(expected);
        }

        [Test]
        public void ReadStocksFromFile()
        {
            var ticker = "qyld";
            var fileContent = File.ReadAllText($@"..\..\..\Files\{ticker}.html");
            fileContent.Should().NotBeNull();
            var stock = new StockDividend();
            StockDividendHistoryLoader.FillProperties(stock, ticker, fileContent);
            stock.Price.Should().NotBe(0.0);
        }
    }
}