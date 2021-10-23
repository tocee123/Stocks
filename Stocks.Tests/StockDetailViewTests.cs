﻿using NUnit.Framework;
using Stocks.Core.Models;
using Stocks.Web.Pages;
using System;
using System.Collections.Generic;

namespace WebDownloading.Test
{
    public class StockDetailViewTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [TestCaseSource(nameof(IsUpcomingStockDividends))]
        public void IsUpcoming_When(StockDividend sd, bool expected)
        {
            var target = new StockDividendTable
            {
                VisibilitySwitch = Common.SwitchToUpcoming
            };
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
            var target = new StockDividendTable
            {
                VisibilitySwitch = Common.SwitchToGraterThan1
            };
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

        [TestCaseSource(nameof(FilterByShortNameStockDividends))]
        public void FilterByShortName_When(StockDividend sd, string shortName, bool expected)
        {
            var target = new StockDividendTable
            {
                ShortNameFilter = shortName
            };
            var result = target.FilterByShortName(sd);
            Assert.AreEqual(expected, result, shortName);
        }

        private static IEnumerable<TestCaseData> FilterByShortNameStockDividends
        {
            get
            {
                yield return new TestCaseData(CerateStockDividend("test"), "t", true);
                yield return new TestCaseData(CerateStockDividend("test"), "e", true);
                yield return new TestCaseData(CerateStockDividend("test"), "es", true);
                yield return new TestCaseData(CerateStockDividend("test"), "st", true);
                yield return new TestCaseData(CerateStockDividend("test"), "test", true);
                yield return new TestCaseData(CerateStockDividend("test"), "f", false);
                yield return new TestCaseData(CerateStockDividend("test"), "", true);
                yield return new TestCaseData(CerateStockDividend("test"), null, true);
            }
        }

        private static StockDividend CerateStockDividend(string shortName)
            => new() { ShortName = shortName };

    }
}