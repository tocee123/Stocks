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

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.AllowAutoRedirect = false; // find out if this site is up and don't follow a redirector
            request.Method = "HEAD";
            try
            {
                var response = request.GetResponse();
                // do something with response.Headers to find out information about the request
            }
            catch (WebException wex)
            {
                //set flag if there was a timeout or some other issues
            }

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