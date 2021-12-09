using NUnit.Framework;
using Stocks.Domain.Helpers;
using System;
using System.Collections.Generic;

namespace WebDownloading.Test
{
    [TestFixture]
    public class DateCalculatorTests
    {
        [TestCaseSource(nameof(WeekendDays))]
        public void CalculateWhenToBuy_WhenExDateIsWeekend_ReturnsFriday(DateTime date)
        {
            var result = DateCalculator.CalculateWhenToBuy(date);
            Assert.AreEqual(DayOfWeek.Friday, result.DayOfWeek);
        }

        [TestCaseSource(nameof(WeekendDays))]
        public void CalculateWhenToSell_WhenExDateIsWeekend_ReturnsMonday(DateTime date)
        {
            var result = DateCalculator.CalculateWhenToSell(date);
            Assert.AreEqual(DayOfWeek.Monday, result.DayOfWeek);
        }

        private static IEnumerable<DateTime> WeekendDays
        {
            get
            {
                yield return new DateTime(2021, 1, 2);
                yield return new DateTime(2021, 1, 3);
            }
        }
    }
}