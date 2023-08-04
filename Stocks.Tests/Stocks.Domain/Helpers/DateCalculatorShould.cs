using Stocks.Domain.Helpers;

namespace Stocks.Test.Stocks.Domain.Helpers
{
    [TestFixture]
    public class DateCalculatorShould
    {
        [TestCaseSource(nameof(WeekendDays))]
        public void ReturnFridayForCalculateWhenToBuyWhenExDateIsWeekend(DateTime date)
        {
            var result = DateCalculator.CalculateWhenToBuy(date);
            Assert.AreEqual(DayOfWeek.Friday, result.DayOfWeek);
        }

        [TestCaseSource(nameof(WeekendDays))]
        public void ReturnMondayForCalculateWhenToSellWhenExDateIsWeekend(DateTime date)
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