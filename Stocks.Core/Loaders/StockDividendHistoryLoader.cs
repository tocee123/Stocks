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

        private static string GetStringByPattern(HtmlNodeCollection htmlNodes, string regexPattern)
        => htmlNodes.Select(p => Regex.Match(p.InnerText, regexPattern))
                .FirstOrDefault(p => p.Success)
                ?.Groups[1].Value;

        internal static void FillProperties(StockDividend stock, string ticker, string html)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var nameNode = htmlDocument.DocumentNode.SelectSingleNode("//div/h4");
            stock.Name = nameNode.InnerText.Trim();
            stock.Ticker = ticker;
            var properties = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='col-md-8 col-xs-12 col-sm-12']").ChildNodes;


            stock.Price = double.Parse(GetStringByPattern(properties, "Last Close Price: \\$([\\d\\.]+)"));

            stock.DividendHistories = htmlDocument.GetElementbyId("dividend_table")
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
