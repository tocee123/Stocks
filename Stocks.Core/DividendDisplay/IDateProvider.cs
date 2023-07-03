namespace Stocks.Core.DividendDisplay
{
    public interface IDateProvider
    {
        DateTime GetToday();
        DateTime GetFirstDayOfCurrentMonth();
        DateTime GetLastDayOfCurrentMonth();
        bool IsWeekend(DateTime date);
    }
}