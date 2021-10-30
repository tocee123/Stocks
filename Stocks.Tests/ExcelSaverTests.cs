using AutoFixture;
using NUnit.Framework;
using Stocks.Core;
using Stocks.Core.Cache;
using Stocks.Core.Excel;
using Stocks.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace WebDownloading.Test
{
    [TestFixture]
    public class ExcelSaverTests
    {
        private IExcelSaver _target;

        [SetUp]
        public void Setup()
        {
            _target = new ExcelSaver();
        }

        [Test]
        public void SaveToExcel_WhenNotEmptyArrayIsGiven_ReturnsExcelFile()
        {
            var fixture = new Fixture();

            var stockDividends = fixture.Create<IEnumerable<StockDividend>>();
            var bytes = _target.SaveToExcel(stockDividends);
            var fileName = @"C:\temp\test1.xlsx";
            File.WriteAllBytes(fileName, bytes);
            Assert.IsTrue(File.Exists(fileName));
        }

        [Test]
        public async Task SaveReal()
        {
            var sr = new StocksRepository(new StocksLoader(new StockDividendHistoryLoader()), new RedisCachedRepository());
            var bytes = _target.SaveToExcel(await sr.GetStocks());
            var fileName = @"C:\temp\test1.xlsx";
            File.WriteAllBytes(fileName, bytes);
            Assert.IsTrue(File.Exists(fileName));
        }
    }
}