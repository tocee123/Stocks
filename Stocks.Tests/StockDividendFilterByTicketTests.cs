using NUnit.Framework;
using Stocks.Core.Models;
using Stocks.Web.HelperClasses.StockFitlers;
using Stocks.Web.Pages;
using System.Collections.Generic;
using System.Linq;

namespace WebDownloading.Test
{
    [TestFixture]
    public class StockDividendFilterByTicketTests
    {
        [Test]
        public void GetFilterArray_ReturnsNotEmptyList()
        {
            var target = new StockDividendFilterByTicket();
            var result = target.GetFilterArray();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(f => f(null)));
            Assert.IsTrue(result.All(f => f(new())));
        }

        [TestCaseSource(nameof(FilterByTickerStockDividends))]
        public void GetFilterArray_ReturnsNotEmptyList(string ticker, bool expected)
        {
            var sd = CerateStockDividendWithTicker("test");
            var target = new StockDividendFilterByTicket(ticker);
            var result = target.GetFilterArray();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(expected, result.Any(f => f(sd)));
        }

        [TestCaseSource(nameof(FilterByTickerStockDividends))]
        public void FilterByShortName_WhenSearchIsGiven_ReturnsExpected(string ticker, bool expected)
        {
            var sd = CerateStockDividendWithTicker("test");
            var target = new StockDividendFilterByTicket(ticker);
            var result = target.FilterByTicker(sd);
            Assert.AreEqual(expected, result, ticker);
        }

        private static StockDividend CerateStockDividendWithTicker(string ticker)
            => new() { Ticker = ticker };

        private static IEnumerable<TestCaseData> FilterByTickerStockDividends
        {
            get
            {
                var input = new (string search, bool expected)[]
                {
                   ("t", true),
                   ("e", true),
                   ("es", true),
                   ("st", true),
                   ("test", true),
                   ("f", false),
                   ("", true),
                   (null, true)
                };

                foreach (var (search, expected) in input)
                {
                    yield return new TestCaseData(search, expected);
                }
            }
        }
    }
}