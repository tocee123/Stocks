using Stocks.Web.HelperClasses.StockFitlers;
using Stocks.Web.Pages;

namespace Stocks.Test.HelperClasses.StockFitlers
{
    public class StockDividendTableStockFilterTests
    {
        [TestCaseSource(nameof(IsUpcomingStockDividends))]
        public void IsUpcoming_When(int daysToAdd, bool expected)
        {
            var sd = CerateStockDividendWithExDate(daysToAdd);
            var target = new StockDividendTableStockFilter("", Common.SwitchToUpcoming, 0);

            var result = target.ShouldDisplay(sd);
            Assert.AreEqual(expected, result, $"ExDate: {sd.LatestDividendHistory.ExDate}");
        }

        private static IEnumerable<TestCaseData> IsUpcomingStockDividends
        {
            get
            {
                var input = new (int daysToAdd, bool expected)[] {
                (1,false),
                (13, true),
                (14, true),
                (0, false),
                (16, false)
                };
                foreach (var (daysToAdd, expected) in input)
                {
                    yield return new TestCaseData(daysToAdd, expected);
                }
            }
        }

        private static StockDividend CerateStockDividendWithExDate(int daysToAdd)
            => new() { DividendHistories = new[] { new DividendHistory { ExDate = DateTime.Today.AddDays(daysToAdd) } } };


        [TestCaseSource(nameof(IsRatioGraterThan1StockDividends))]
        public void IsRatioGraterThan1_When(double price, double amount, bool expected)
        {
            var sd = CerateStockDividendWithPriceAndAmount(price, amount);
            var target = new StockDividendTableStockFilter("", Common.SwitchToGraterThan1, 0);

            var result = target.ShouldDisplay(sd);
            Assert.AreEqual(expected, result, $"DividendToPrice: {sd.DividendToPrice}");
        }

        private static IEnumerable<TestCaseData> IsRatioGraterThan1StockDividends
        {
            get
            {
                var input = new (double price, double amount, bool expected)[]
                   {
                       (100,1, true),
                       (110,1, false),
                       (90,1,true)
                   };
                foreach (var (price, amount, expected) in input)
                {
                    yield return new TestCaseData(price, amount, expected);
                }
            }
        }

        private static StockDividend CerateStockDividendWithPriceAndAmount(double price, double amount)
            => new() { DividendHistories = new[] { new DividendHistory { Amount = amount } }, Price = price };

        [TestCaseSource(nameof(FilterByTickerStockDividends))]
        public void FilterByShortName_When(string ticker, bool expected)
        {
            var sd = CerateStockDividendWithTicker("test");
            var target = new StockDividendTableStockFilter(ticker, null, 0);
            var result = target.ShouldDisplay(sd);
            Assert.AreEqual(expected, result, ticker);
        }

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

        [TestCaseSource(nameof(FilterByMaxPriceStockDividends))]
        public void ShouldDisplay_WhenPriceIsSet_FiltersAccordingly(int price, int maxPrice, bool expected)
        {
            var stockDividend = CerateStockDividendWithPrie(price);
            var target = new StockDividendTableStockFilter(null, null, maxPrice);
            var result = target.ShouldDisplay(stockDividend);
            Assert.AreEqual(expected, result, nameof(maxPrice));
        }

        [TestCaseSource(nameof(FilterByMaxPriceStockDividends))]
        public void FilterByMaxPrice_WhenPriceIsSet_FiltersAccordingly(int price, int maxPrice, bool expected)
        {
            var stockDividend = CerateStockDividendWithPrie(price);
            var target = new StockDividendTableStockFilter(null, null, maxPrice);
            var result = target.ShouldDisplay(stockDividend);
            Assert.AreEqual(expected, result, nameof(maxPrice));
        }

        private static IEnumerable<TestCaseData> FilterByMaxPriceStockDividends
        {
            get
            {
                var input = new (int price, int max, bool expected)[]
                {
                    (10, 10, true),
                    (10, 5, false),
                    (10, 0, true),
                    (10, 1, false),
                    (0,0, true)
                };

                foreach (var (price, max, expected) in input)
                {
                    yield return new TestCaseData(price, max, expected);
                }
            }
        }

        private static StockDividend CerateStockDividendWithTicker(string ticker)
            => new() { Ticker = ticker };

        private static StockDividend CerateStockDividendWithPrie(int price)
            => new() { Price = price };
    }
}