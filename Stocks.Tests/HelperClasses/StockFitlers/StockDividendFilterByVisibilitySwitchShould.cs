﻿using Stocks.Web.HelperClasses.StockFitlers;
using WebPagesCommon = Stocks.Web.Pages.Common;
namespace Stocks.Test.HelperClasses.StockFitlers
{
    [TestFixture]
    public class StockDividendFilterByVisibilitySwitchShould
    {
        [Test]
        public void Filter_ReturnsNotEmptyList()
        {
            var target = new StockDividendFilterByVisibilitySwitch();
            target.Filter(null).Should().BeTrue();
            target.Filter(new()).Should().BeTrue();
        }

        [TestCaseSource(nameof(IsUpcomingStockDividends))]
        public void ReturnTrueWhenWhenTheDateIsBetweenTheRange(int daysToAdd, bool expected)
        {
            var sd = CerateStockDividendWithExDate(daysToAdd);
            var result = StockDividendFilterByVisibilitySwitch.IsUpcoming(sd);
            result.Should().Be(expected, because: $"ExDate: {sd.LatestDividendHistory.ExDate}");
        }

        [TestCaseSource(nameof(IsUpcomingStockDividends))]
        public void Filter_ReturnTrueWhenWhenTheDateIsBetweenTheRange(int daysToAdd, bool expected)
        {
            var sd = CerateStockDividendWithExDate(daysToAdd);
            var target = new StockDividendFilterByVisibilitySwitch(WebPagesCommon.SwitchToUpcoming);

            var result = target.Filter(sd);


            result.Should().Be(expected, because: $"ExDate: {sd.LatestDividendHistory.ExDate}");
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
            var result = StockDividendFilterByVisibilitySwitch.IsRatioGraterThan1(sd);

            result.Should().Be(expected, because: $"DividendToPrice: {sd.DividendToPrice}");
        }

        [TestCaseSource(nameof(IsRatioGraterThan1StockDividends))]
        public void Filter_WhenRatioisBetweenThreshold_ReturnTrue(double price, double amount, bool expected)
        {
            var sd = CerateStockDividendWithPriceAndAmount(price, amount);
            var target = new StockDividendFilterByVisibilitySwitch(WebPagesCommon.SwitchToGraterThan1);

            var result = target.Filter(sd);

            result.Should().Be(expected, because: $"DividendToPrice: {sd.DividendToPrice}");
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
        public void Filter_WhenHasSpecial_ReturnTrue(bool isSpecial, bool expected)
        {
            var sd = CerateStockDividendSpecial(isSpecial);
            var target = new StockDividendFilterByVisibilitySwitch(WebPagesCommon.HasSpecial);

            var result = target.Filter(sd);

            result.Should().Be(expected, because: $"Special: {sd.HasSpecial}");
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