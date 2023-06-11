using AutoFixture;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NUnit.Framework;
using Stocks.Core.Excel;
using Stocks.Core.Loaders;
using Stocks.Core.Repositories;
using Stocks.Domain.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace WebDownloading.Test
{
    [TestFixture]
    public class ExcelSaverTests
    {
        //private IExcelSaver _target;

        //[SetUp]
        //public void Setup()
        //{
        //    _target = new ExcelSaver();
        //}

        //TODO
        //[Test]
        public void SaveToExcel_WhenNotEmptyArrayIsGiven_ReturnsExcelFile()
        {
            var fixture = new Fixture();

            var stockDividends = fixture.Create<IEnumerable<StockDividend>>();
            var bytes = _target.SaveToExcel(stockDividends);
            var fileName = @"C:\temp\test1.xlsx";
            File.WriteAllBytes(fileName, bytes);
            Assert.IsTrue(File.Exists(fileName));
        }

        //TODO
        //[Test]
        public async Task SaveReal()
        {
            var configurationSub = Substitute.For<IConfiguration>();
            var sr = new StocksRepository(new StocksLoader(new StockDividendHistoryLoader()), new StocksOfInterestRespository());
            var bytes = _target.SaveToExcel(await sr.GetStocksAsync());
            var fileName = @"C:\temp\test1.xlsx";
            File.WriteAllBytes(fileName, bytes);
            Assert.IsTrue(File.Exists(fileName));
        }
    }
}