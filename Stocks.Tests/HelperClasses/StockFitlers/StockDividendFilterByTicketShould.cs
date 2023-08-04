using Stocks.Web.HelperClasses.StockFitlers;

namespace Stocks.Test.HelperClasses.StockFitlers
{
    [TestFixture]
    public class StockDividendFilterByTicketShould
    {
        [Test]
        public void ReturnTrueWhenNullOrOneElementInList()
        {
            var target = new StockDividendFilterByTicker();
            Assert.IsTrue(target.Filter(null));
            Assert.IsTrue(target.Filter(new()));
        }

        [TestCaseSource(nameof(FilterByTickerStockDividends))]
        public void ReturnsFilteredTickers(string ticker, bool expected)
        {
            var target = new StockDividendFilterByTicker(ticker);
            Filter(target.Filter, ticker, expected);
        }

        [TestCaseSource(nameof(FilterByTickerStockDividends))]
        public void ReturnsShouldFilteredTickers(string ticker, bool expected)
        {
            var target = new StockDividendFilterByTicker(ticker);
            Filter(target.ShouldFilter, ticker, expected);
        }

        private static void Filter(Func<StockDividend, bool> filter, string ticker, bool expected)
        {
            var sd = CerateStockDividendWithTicker("test");
            var target = new StockDividendFilterByTicker(ticker);
            var result = filter(sd);
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