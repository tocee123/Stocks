using Stocks.Web.HelperClasses.StockFitlers;
using WebPagesCommon = Stocks.Web.Pages.Common;

namespace Stocks.Test.HelperClasses.StockFitlers
{
    public class StockDividendTableStockFilterShould
    {
        [TestCaseSource(nameof(IsUpcomingStockDividends))]
        public void ReturnTrueWhenDateIsMoreThan1AndLessThan15DaysAhead(int daysToAdd, bool expected)
        {
            var sd = CerateStockDividendWithExDate(daysToAdd);
            var target = new StockDividendTableStockFilter("", WebPagesCommon.SwitchToUpcoming, 0);

            var result = target.ShouldDisplay(sd);
            result.Should().Be(expected, because: $"ExDate: {sd.LatestDividendHistory.ExDate}");
        }

        [TestCaseSource(nameof(IsRatioGraterThan1StockDividends))]
        public void ReturnsTrueWhenRatioIsGreaterThan1(double price, double amount, bool expected)
        {
            var sd = CerateStockDividendWithPriceAndAmount(price, amount);
            var target = new StockDividendTableStockFilter("", WebPagesCommon.SwitchToGraterThan1, 0);

            var result = target.ShouldDisplay(sd);
            result.Should().Be(expected, because: $"DividendToPrice: {sd.DividendToPrice}");
        }

        [TestCaseSource(nameof(FilterByTickerStockDividends))]
        public void ReturnTrueWhenTickerContainsShortName(string ticker, bool expected)
        {
            var sd = CerateStockDividendWithTicker("test");
            var target = new StockDividendTableStockFilter(ticker, null, 0);
            var result = target.ShouldDisplay(sd);
            result.Should().Be(expected, because: ticker);
        }

        [TestCaseSource(nameof(FilterByMaxPriceStockDividends))]
        public void ReturnTrueWhenPriceIsLessThanMaxPrice(int price, int maxPrice, bool expected)
        {
            var stockDividend = CerateStockDividendWithPrie(price);
            var target = new StockDividendTableStockFilter(null, null, maxPrice);
            var result = target.ShouldDisplay(stockDividend);
            result.Should().Be(expected, because: nameof(maxPrice));
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