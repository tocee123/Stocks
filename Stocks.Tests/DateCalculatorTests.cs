using NUnit.Framework;
using Stocks.Core;
using System;
using System.Collections.Generic;

namespace WebDownloading.Test
{
    [TestFixture]
    public class DateCalculatorTests
    {
        DateCalculator _target = new();

        [TestCaseSource(nameof(WeekendDays))]
        public void CalculateWhenToBuy_WhenExDateIsWeekend_ReturnsFriday(DateTime date)
        {
            var result = _target.CalculateWhenToBuy(date);
            Assert.AreEqual(DayOfWeek.Friday, result.DayOfWeek);
        }

        private static IEnumerable<DateTime> WeekendDays
        {
            get
            {
                yield return new DateTime(2021, 1, 2);
                yield return new DateTime(2021, 1, 3);
            }
        }

        [TestCaseSource(nameof(WeekendDays))]
        public void CalculateWhenToSell_WhenExDateIsWeekend_ReturnsMonday(DateTime date)
        {
            var result = _target.CalculateWhenToSell(date);
            Assert.AreEqual(DayOfWeek.Monday, result.DayOfWeek);
        }
        private static IEnumerable<DateTime> CalculateWhenToBuy_WhenExDateIsWeekend_ReturnsFriday_Source
        {
            get
            {
                yield return new DateTime(2021, 1, 2);
                yield return new DateTime(2021, 1, 3);
            }
        }
    }
}