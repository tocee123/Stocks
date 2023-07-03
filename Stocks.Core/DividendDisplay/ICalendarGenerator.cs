namespace Stocks.Core.DividendDisplay
{
    public interface ICalendarGenerator
    {
        IEnumerable<IEnumerable<DisplayDay>> GenerateMonth();
    }
}