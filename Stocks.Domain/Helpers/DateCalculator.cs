namespace Stocks.Domain.Helpers
{
    public class DateCalculator
    {
        private static readonly int _yesterday = -1;
        private static readonly int _tomorrow = 1;

        public static DateTime CalculateWhenToBuy(DateTime date)
        => MoveDayOutFromWeekend(date, _yesterday);

        public static DateTime CalculateWhenToSell(DateTime date)
        => MoveDayOutFromWeekend(date, _tomorrow);

        private static DateTime MoveDayOutFromWeekend(DateTime date, int daysToMove)
        {
            try
            {
                DateTime result = date.AddDays(daysToMove);
                while (IsWeekend(result))
                    result = result.AddDays(daysToMove);
                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(date.ToShortDateString(), ex);
            }
        }

        readonly static DayOfWeek[] _weekendDays = new[] { DayOfWeek.Sunday, DayOfWeek.Saturday };

        private static bool IsWeekend(DateTime date)
            => _weekendDays.Any(wd => wd == date.DayOfWeek);
    }
}
