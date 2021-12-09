using HtmlAgilityPack;
using Stocks.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Stocks.Core.Loaders
{
    public class StockDividendHistoryLoader : IStockDividendHistoryLoader
    {
        public async Task<StockDividend> DownloadStockHistoryAsync(string ticker)
        {
            try
            {
                var stock = new StockDividend();
                using var client = new WebClient();
                var url = CreateUrlToYCharts(ticker);
                var (pageExists, resultUrl) = await DoesPageExist(url);
                if (pageExists)
                {
                    await FillProperties(stock, client, resultUrl);
                }
                return stock;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private static string CreateUrlToYCharts(string ticker)
            => $"https://ycharts.com/companies/{ticker}/dividend";

        private static async Task FillProperties(StockDividend stock, WebClient client, Uri resultUri)
        {
            string htmlCode = await client.DownloadStringTaskAsync(resultUri);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlCode);

            var nameNode = htmlDocument.DocumentNode.SelectSingleNode("//h3[@class='securityName']");
            if (!(nameNode?.HasChildNodes ?? false))
                return;
            stock.Name = nameNode.ChildNodes[1].InnerText;
            stock.Ticker = nameNode.ChildNodes[3].InnerText;
            stock.Price = double.Parse(htmlDocument.DocumentNode.SelectSingleNode("//span[@class='upDn']").InnerText);
            stock.DividendHistories = htmlDocument.DocumentNode.SelectSingleNode("//table[@class='histDividendDataTable']")
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
                using var client = new HttpClient();
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
