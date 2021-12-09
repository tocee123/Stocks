using NUnit.Framework;
using Stocks.Domain.Models;
using Stocks.Web.HelperClasses.StockFitlers;
using System.Collections.Generic;

namespace Stocks.Test.HelperClasses.StockFitlers
{
    [TestFixture]
    public class StockDividendFilterByTicketTests
    {
        [Test]
        public void Filter_ReturnsNotEmptyList()
        {
            var target = new StockDividendFilterByTicker();
            Assert.IsTrue(target.Filter(null));
            Assert.IsTrue(target.Filter(new()));
        }

        [TestCaseSource(nameof(FilterByTickerStockDividends))]
        public void Filter_ReturnsNotEmptyList(string ticker, bool expected)
        {
            var sd = CerateStockDividendWithTicker("test");
            var target = new StockDividendFilterByTicker(ticker);
            var result = target.Filter(sd);
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);
        }

        [TestCaseSource(nameof(FilterByTickerStockDividends))]
        public void FilterByShortName_WhenSearchIsGiven_ReturnsExpected(string ticker, bool expected)
        {
            var sd = CerateStockDividendWithTicker("test");
            var target = new StockDividendFilterByTicker(ticker);
            var result = target.ShouldFilter(sd);
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