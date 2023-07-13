using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Stocks.Core.Extensions;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace Stocks.Core.Loaders
{
    public class StockDividendHistoryLoader : IStockDividendHistoryLoader
    {
        ILogger<StockDividendHistoryLoader> _logger;

        public StockDividendHistoryLoader(ILogger<StockDividendHistoryLoader> logger)
        {
            _logger = logger;
        }

        public async Task<StockDividend> DownloadStockHistoryAsync(string ticker)
        {
            var stock = new StockDividend();
            try
            {
                var url = CreateUrlToYCharts(ticker);
                var (pageExists, resultUrl) = await DoesPageExist(url);
                if (pageExists)
                {
                    var html = await DownloadHtml(resultUrl);
                    FillProperties(stock, ticker, html);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while processing {ticker}\n{ex.Message}");
            }
            return stock;
        }

        private static async Task<string> DownloadHtml(Uri resultUri)
        {
            using var client = new WebClient();
            string html = await client.DownloadStringTaskAsync(resultUri);
            return html;
        }

        private static string CreateUrlToYCharts(string ticker)
            => $"https://dividendhistory.org/payout/{ticker}/";

        internal static void FillProperties(StockDividend stock, string ticker, string html)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var nameNode = htmlDocument.DocumentNode.SelectSingleNode("//div/h4");
            if (nameNode == null)
            {
                return;
            }
            stock.Name = nameNode.InnerText.Trim();
            stock.Ticker = ticker;
            stock.Price = GetPrice(htmlDocument);
            stock.DividendHistories = GetDividendHistories(htmlDocument);
            stock.IsCorrectlyDownloaded = true;
        }

        internal async static Task<(bool, Uri)> DoesPageExist(string url)
        {
            var result = false;
            if (IsCorrectUrl(url, out var resultUri))
            {
                var handler = new HttpClientHandler()
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                };
                using var client = new HttpClient(handler);
                client.DefaultRequestHeaders.Add("Accept", "*/*");
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
                var response = await client.GetAsync(resultUri.AbsoluteUri);
                result = response.IsSuccessStatusCode;
            }
            return (result, resultUri);
        }

        internal static bool IsCorrectUrl(string url, out Uri resultUri)
        {
            bool startsWithHttp() => !Regex.IsMatch(url, @"^https?:\/\/", RegexOptions.IgnoreCase);
            bool endsWithExtension() => Regex.IsMatch(url, @".*\.\w{2,}$", RegexOptions.IgnoreCase);

            if (startsWithHttp() && endsWithExtension())
                url = $"http://{url}";

            if (Uri.TryCreate(url, UriKind.Absolute, out resultUri))
                return resultUri.Scheme == Uri.UriSchemeHttp ||
                        resultUri.Scheme == Uri.UriSchemeHttps;

            return false;
        }

        private static double GetPrice(HtmlDocument htmlDocument)
        => htmlDocument.DocumentNode.SelectSingleNode("//div[@class='col-md-8 col-xs-12 col-sm-12']")
               .ChildNodes
               .Select(p => Regex.Match(p.InnerText, "Last Close Price: \\$([\\d\\.]+)"))
               .FirstOrDefault(p => p.Success)
               ?.Groups[1].Value
               .ToDouble()
               ?? default;

        private static IEnumerable<DividendHistory> GetDividendHistories(HtmlDocument htmlDocument)
        => htmlDocument.GetElementbyId("dividend_table")
                .Descendants("tr")
                .Skip(1)
                .Where(tr => tr.Elements("td").Count() > 1)
                .Select(tr => ToDividendHistory(tr.Elements("td")))
                .ToList();

        private static DividendHistory ToDividendHistory(IEnumerable<HtmlNode> tdNodes)
        {
            DateTime parseDate(string s)
            {
                DateTime.TryParse(s, out var date);
                return date;
            };
            var tdNodeList = tdNodes.ToList();
            var i = 0;

            var exDate = parseDate(tdNodeList[i++].InnerText);
            var payDate = parseDate(tdNodeList[i++].InnerText);
            var amount = double.Parse(Regex.Match(tdNodeList[i++].InnerText, "\\$([\\d\\.]+)").Groups[1].Value);
            var type = tdNodeList[i].InnerText;
            return new DividendHistory
            {
                ExDate = exDate,
                RecordDate = exDate.AddDays(1),
                PayDate = payDate,
                DeclarationDate = exDate.AddDays(-10),
                Type = type,
                Amount = amount
            };
        }
    }
}
