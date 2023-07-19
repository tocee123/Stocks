namespace Stocks.Core.DividendDisplay
{
    public interface ICalendarGenerator
    {
        Task<IEnumerable<IEnumerable<DisplayDay>>> GenerateMonthAsync();
        Task<double> GetMaximumDividendYield();
        DateTime Today { get; }
    }
}