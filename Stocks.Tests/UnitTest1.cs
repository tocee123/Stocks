using NUnit.Framework;
using Stocks.Core;
using Stocks.Core.Excel;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace WebDownloading.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test2()
        {
            var url = "https://www.etoro.com/markets/agnc";

            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
            // Using WebClient
            string result1 = new WebClient().DownloadString(url);

        }

        [Test]
        public async Task Test1()
        {
            var excelWriter = new StockExcelWriter(new StocksRepository(new StocksLoader(new StockDividendHistoryLoader()), new RedisCache()), new ExcelSaver());
            var bytes = await excelWriter.WriteToExcelAsync();
            await File.WriteAllBytesAsync(@"C:\temp\test1.xlsx", bytes);
        }
    }
}