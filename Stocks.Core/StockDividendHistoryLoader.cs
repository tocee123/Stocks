using HtmlAgilityPack;
using Stocks.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Stocks.Core
{
    public class StockDividendHistoryLoader : IStockDividendHistoryLoader
    {
        public async Task<StockDividend> DownloadStockHistoryAsync(string stockShortName)
        {
            var stock = new StockDividend();
            using var client = new WebClient();
            try
            {
                string htmlCode = await client.DownloadStringTaskAsync($"https://ycharts.com/companies/{stockShortName}/dividend");
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlCode);

                var nameNode = htmlDocument.DocumentNode.SelectSingleNode("//h3[@class='securityName']");
                stock.Name = nameNode.ChildNodes[1].InnerText;
                stock.ShortName = nameNode.ChildNodes[3].InnerText;
                stock.Price = double.Parse(htmlDocument.DocumentNode.SelectSingleNode("//span[@class='upDn']").InnerText);
                stock.DividendHistories = htmlDocument.DocumentNode.SelectSingleNode("//table[@class='histDividendDataTable']")
                    .Descendants("tr")
                    .Skip(1)
                    .Where(tr => tr.Elements("td").Count() > 1)
                    .Select(tr => ToDividendHistory(tr.Elements("td")))
                    .ToList();
                stock.IsCorrectlyDownloaded = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing {stockShortName}:\n{ex.Message}");
            }
            return stock;
        }

        private static DividendHistory ToDividendHistory(IEnumerable<HtmlNode> tdNodes)
        {
            var tdNodeList = tdNodes.ToList();
            var i = 0;
            return new DividendHistory
            {
                ExDate = DateTime.Parse(tdNodeList[i++].InnerText),
                RecordDate = DateTime.Parse(tdNodeList[i++].InnerText),
                PayDate = DateTime.Parse(tdNodeList[i++].InnerText),
                DeclarationDate = DateTime.Parse(tdNodeList[i++].InnerText),
                Type = tdNodeList[i++].InnerText,
                Amount = double.Parse(tdNodeList[i].InnerText)
            };
        }
    }
}
