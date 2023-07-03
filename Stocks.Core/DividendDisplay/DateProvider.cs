namespace Stocks.Core.DividendDisplay;

public class DateProvider : IDateProvider
{
    private Lazy<DateTime> FirstDayOfCurrentMonth;
    private Lazy<DateTime> LastDayOfCurrentMonth;

    public DateTime GetToday()
        => DateTime.Today;

    private static readonly IEnumerable<DayOfWeek> _weekend = new[] { DayOfWeek.Saturday, DayOfWeek.Sunday };

    public bool IsWeekend(DateTime date)
    => _weekend.Contains(date.DayOfWeek);

    public DateTime GetFirstDayOfCurrentMonth()
    {
        FirstDayOfCurrentMonth = FirstDayOfCurrentMonth ?? new Lazy<DateTime>(() => new DateTime(GetToday().Year, GetToday().Month, 1));
        return FirstDayOfCurrentMonth.Value;
    }

    public DateTime GetLastDayOfCurrentMonth()
    {
        LastDayOfCurrentMonth = LastDayOfCurrentMonth ?? new Lazy<DateTime>(() => new DateTime(GetToday().Year, GetToday().Month, DateTime.DaysInMonth(GetToday().Year, GetToday().Month)));
        return LastDayOfCurrentMonth.Value;
    }
}
