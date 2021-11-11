using NUnit.Framework;
using Stocks.Core.Models;
using Stocks.Web.Pages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebDownloading.Test
{
    public class StockDividendTableStockFilterTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [TestCaseSource(nameof(IsUpcomingStockDividends))]
        public void IsUpcoming_When(StockDividend sd, bool expected)
        {
            var target = new StockDividendTableStockFilter("", Common.SwitchToUpcoming, 0);

            var result = target.IsUpcoming(sd);
            Assert.AreEqual(expected, result, $"ExDate: {sd.LatestDividendHistory.ExDate}");
        }

        private static IEnumerable<TestCaseData> IsUpcomingStockDividends
        {
            get
            {
                yield return new TestCaseData(CerateStockDividend(1), false);
                yield return new TestCaseData(CerateStockDividend(13), true);
                yield return new TestCaseData(CerateStockDividend(14), true);
                yield return new TestCaseData(CerateStockDividend(0), false);
                yield return new TestCaseData(CerateStockDividend(16), false);
            }
        }

        private static StockDividend CerateStockDividend(int daysToAdd)
            => new() { DividendHistories = new[] { new DividendHistory { ExDate = DateTime.Today.AddDays(daysToAdd) } } };


        [TestCaseSource(nameof(IsRatioGraterThan1StockDividends))]
        public void IsRatioGraterThan1_When(StockDividend sd, bool expected)
        {
            var target = new StockDividendTableStockFilter("", Common.SwitchToGraterThan1, 0);

            var result = target.IsRatioGraterThan1(sd);
            Assert.AreEqual(expected, result, $"DividendToPrice: {sd.DividendToPrice}");
        }

        private static IEnumerable<TestCaseData> IsRatioGraterThan1StockDividends
        {
            get
            {
                yield return new TestCaseData(CerateStockDividend(100, 1), true);
                yield return new TestCaseData(CerateStockDividend(110, 1), false);
                yield return new TestCaseData(CerateStockDividend(90, 1), true);
            }
        }

        private static StockDividend CerateStockDividend(double price, double amount)
            => new() { DividendHistories = new[] { new DividendHistory { Amount = amount } }, Price = price };

        [TestCaseSource(nameof(FilterByTickerStockDividends))]
        public void FilterByShortName_When(StockDividend sd, string ticker, bool expected)
        {
            var target = new StockDividendTableStockFilter(ticker, null, 0);
            var result = target.FilterByTicker(sd);
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

                foreach (var item in input)
                {
                    yield return new TestCaseData(CerateStockDividendWithTicker("test"), item.search, item.expected);
                }
            }
        }

        [TestCaseSource(nameof(FilterByMaxPriceStockDividends))]
        public void ShouldDisplay_WhenPriceIsSet_FiltersAccordingly(StockDividend sd, int maxPrice, bool expected)
        {
            var target = new StockDividendTableStockFilter(null, null, maxPrice);
            var result = target.ShouldDisplay(sd);
            Assert.AreEqual(expected, result, nameof(maxPrice));
        }

        [TestCaseSource(nameof(FilterByMaxPriceStockDividends))]
        public void FilterByMaxPrice_WhenPriceIsSet_FiltersAccordingly(StockDividend sd, int maxPrice, bool expected)
        {
            var target = new StockDividendTableStockFilter(null, null, maxPrice);
            var result = target.FilterByMaxPrice(sd);
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

                foreach (var item in input)
                {
                    yield return new TestCaseData(CerateStockDividendWithPrie(item.price), item.max, item.expected);
                }
            }
        }

        private static StockDividend CerateStockDividendWithTicker(string ticker)
            => new() { Ticker = ticker };

        private static StockDividend CerateStockDividendWithPrie(int price)
            => new() { Price = price };
    }
}