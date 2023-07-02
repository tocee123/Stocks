using HtmlAgilityPack;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace Stocks.Core.Loaders
{
    public class StockDividendHistoryEmptyLoader : IStockDividendHistoryLoader
    {
        public async Task<StockDividend> DownloadStockHistoryAsync(string ticker)
        {
            return new StockDividend { IsCorrectlyDownloaded = true, Price = 5, Name = "Tony Stock", Ticker = "TSK", DividendHistories = new[] { new DividendHistory { Amount = 10, DeclarationDate = DateTime.Now, ExDate = DateTime.Now, PayDate = DateTime.Now, RecordDate = DateTime.Now, Type = "test" } } };
        }
    }

    public class StockDividendHistoryLoader : IStockDividendHistoryLoader
    {
        public async Task<StockDividend> DownloadStockHistoryAsync(string ticker)
        {
            try
            {
                var stock = new StockDividend();

                var url = CreateUrlToYCharts(ticker);
                var (pageExists, resultUrl) = await DoesPageExist(url);
                if (pageExists)
                {
                    var html = await DownloadString(resultUrl);
                    FillProperties(stock, ticker, html);
                }
                return stock;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private static async Task<string> DownloadString(Uri resultUri)
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
            stock.Name = nameNode.InnerText.Trim();
            stock.Ticker = ticker;
            var properties = htmlDocument.DocumentNode.SelectSingleNode("////div[@class='panel-content']");
            stock.Price = double.Parse(htmlDocument.DocumentNode.SelectSingleNode("//span[@class='index-rank-value']").InnerText);
            var divParentContent = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='panel-content']");

            stock.DividendHistories = htmlDocument.DocumentNode.SelectSingleNode("//table[@class='table']")
                .Descendants("tr")
                .Skip(1)
                .Where(tr => tr.Elements("td").Count() > 1)
                .Select(tr => ToDividendHistory(tr.Elements("td")))
                .ToList();
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
                client.Timeout = TimeSpan.FromSeconds(2);
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

        private static DividendHistory ToDividendHistory(IEnumerable<HtmlNode> tdNodes)
        {
            DateTime parse(string s)
            {
                DateTime.TryParse(s, out var date);
                return date;
            };
            var tdNodeList = tdNodes.ToList();
            var i = 0;
            return new DividendHistory
            {
                ExDate = parse(tdNodeList[i++].InnerText),
                RecordDate = parse(tdNodeList[i++].InnerText),
                PayDate = parse(tdNodeList[i++].InnerText),
                DeclarationDate = parse(tdNodeList[i++].InnerText),
                Type = tdNodeList[i++].InnerText,
                Amount = double.Parse(tdNodeList[i].InnerText)
            };
        }
    }
}
