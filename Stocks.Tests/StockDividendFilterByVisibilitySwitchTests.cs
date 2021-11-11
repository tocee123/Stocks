using NUnit.Framework;
using Stocks.Core.Models;
using Stocks.Web.HelperClasses.StockFitlers;
using Stocks.Web.Pages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebDownloading.Test
{
    [TestFixture]
    public class StockDividendFilterByVisibilitySwitchTests
    {
        [Test]
        public void GetFilterArray_ReturnsNotEmptyList()
        {
            var target = new StockDividendFilterByVisibilitySwitch();
            Assert.IsTrue(target.Filter(null));
            Assert.IsTrue(target.Filter(new()));
        }

        [TestCaseSource(nameof(IsUpcomingStockDividends))]
        public void IsUpcoming_WhenTheDateIsBetweenTheRange_ReturnsTrue(int daysToAdd, bool expected)
        {
            var sd = CerateStockDividendWithExDate(daysToAdd);
            var target = new StockDividendFilterByVisibilitySwitch(Common.SwitchToUpcoming);

            var result = target.IsUpcoming(sd);
            Assert.AreEqual(expected, result, $"ExDate: {sd.LatestDividendHistory.ExDate}");
        }

        [TestCaseSource(nameof(IsUpcomingStockDividends))]
        public void GetFilterArray_WhenTheDateIsBetweenTheRange_ReturnsTrue(int daysToAdd, bool expected)
        {
            var sd = CerateStockDividendWithExDate(daysToAdd);
            var target = new StockDividendFilterByVisibilitySwitch(Common.SwitchToUpcoming);

            var result = target.Filter(sd);

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
        public void IsRatioGraterThan1_WhenRatioisBetweenThreshold_ReturnTrue(double price, double amount, bool expected)
        {
            var sd = CerateStockDividendWithPriceAndAmount(price, amount);
            var target = new StockDividendFilterByVisibilitySwitch(Common.SwitchToGraterThan1);

            var result = target.IsRatioGraterThan1(sd);
            Assert.AreEqual(expected, result, $"DividendToPrice: {sd.DividendToPrice}");
        }

        [TestCaseSource(nameof(IsRatioGraterThan1StockDividends))]
        public void GetFilterArray_WhenRatioisBetweenThreshold_ReturnTrue(double price, double amount, bool expected)
        {
            var sd = CerateStockDividendWithPriceAndAmount(price, amount);
            var target = new StockDividendFilterByVisibilitySwitch(Common.SwitchToGraterThan1);

            var result = target.Filter(sd);

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

        [TestCaseSource(nameof(HasSpecialStockDividends))]
        public void GetFilterArray_WhenHasSpecial_ReturnTrue(bool isSpecial, bool expected)
        {
            var sd = CerateStockDividendSpecial(isSpecial);
            var target = new StockDividendFilterByVisibilitySwitch(Common.HasSpecial);

            var result = target.Filter(sd);

            Assert.AreEqual(expected, result, $"Special: {sd.HasSpecial}");
        }

        private static IEnumerable<TestCaseData> HasSpecialStockDividends

        {
            get
            {
                var input = new (bool isSpecial, bool expected)[]
                   {
                       (true, true),
                       (false, false)
                   };
                foreach (var (isSpecial, expected) in input)
                {
                    yield return new TestCaseData(isSpecial, expected);
                }
            }
        }

        private static StockDividend CerateStockDividendSpecial(bool isSpecial)
            => new() { DividendHistories = new[] { new DividendHistory { Type = isSpecial ? "Special" : "random" } } };
    }
}