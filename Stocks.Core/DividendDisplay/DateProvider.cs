namespace Stocks.Core.DividendDisplay;

public class DateProvider : IDateProvider
{
    public DateTime GetToday()
        => DateTime.Today;

    private static readonly IEnumerable<DayOfWeek> _weekend = new[] { DayOfWeek.Saturday, DayOfWeek.Sunday };

    public bool IsWeekend(DateTime date)
    => _weekend.Contains(date.DayOfWeek);
}
