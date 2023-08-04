﻿using Stocks.Web.HelperClasses.StockFitlers;

namespace Stocks.Test.HelperClasses.StockFitlers
{
    [TestFixture]
    public class StockDividendFilterByMaxPriceShould
    {
        [Test]
        public void ReturnTrueWhenNullOrOneElementInList()
        {
            var target = new StockDividendFilterByMaxPrice();
            Assert.IsTrue(target.Filter(null));
            Assert.IsTrue(target.Filter(new()));
        }

        [TestCaseSource(nameof(FilterByMaxPriceStockDividends))]
        public void FilterOutDividendsWherePriceIsLessThenMaxPrice(int price, int maxPrice, bool expected)
        {
            var sd = CerateStockDividendWithPrice(price);
            var target = new StockDividendFilterByMaxPrice(maxPrice);
            var result = target.Filter(sd);
            Assert.AreEqual(expected, result);
        }

        private static StockDividend CerateStockDividendWithPrice(int price)
            => new() { Price = price };

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
    }
}