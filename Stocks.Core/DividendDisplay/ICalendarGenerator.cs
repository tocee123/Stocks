namespace Stocks.Core.DividendDisplay
{
    public interface ICalendarGenerator
    {
        Task<IEnumerable<IEnumerable<DisplayDay>>> GenerateMonthAsync();
        DateTime Today { get; }
    }
}