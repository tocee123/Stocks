﻿using NUnit.Framework;
using Stocks.Core.Models;
using Stocks.Web.Pages;
using System.Collections.Generic;
using System.Linq;

namespace WebDownloading.Test
{
    [TestFixture]
    public class StockDividendFilterByMaxPriceTests
    {
        [Test]
        public void GetFilterArray_ReturnsNotEmptyList()
        {
            var target = new StockDividendFilterByMaxPrice();
            var result = target.GetFilterArray();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(f => f(null)));
            Assert.IsTrue(result.All(f => f(new())));
        }

        [TestCaseSource(nameof(FilterByMaxPriceStockDividends))]
        public void GetFilterArray_ReturnsNotEmptyList(int price, int maxPrice, bool expected)
        {
            var sd = CerateStockDividendWithPrice(price);
            var target = new StockDividendFilterByMaxPrice(maxPrice);
            var result = target.GetFilterArray();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(expected, result.Any(f => f(sd)));
        }

        [TestCaseSource(nameof(FilterByMaxPriceStockDividends))]
        public void ShouldDisplay_WhenPriceIsSet_FiltersAccordingly(int price, int maxPrice, bool expected)
        {
            var sd = CerateStockDividendWithPrice(price);
            var target = new StockDividendFilterByMaxPrice(maxPrice);
            var result = target.GetFilterArray();
            Assert.AreEqual(expected, result.Any(f => f(sd)));
        }

        [TestCaseSource(nameof(FilterByMaxPriceStockDividends))]
        public void FilterByMaxPrice_WhenPriceIsSet_FiltersAccordingly(int price, int maxPrice, bool expected)
        {
            var sd = CerateStockDividendWithPrice(price);
            var target = new StockDividendFilterByMaxPrice(maxPrice);
            var result = target.FilterByMaxPrice(sd);
            Assert.AreEqual(expected, result, nameof(maxPrice));
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