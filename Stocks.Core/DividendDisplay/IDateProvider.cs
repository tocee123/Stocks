namespace Stocks.Core.DividendDisplay
{
    public interface IDateProvider
    {
        DateTime GetToday();
        bool IsWeekend(DateTime date);
    }
}