namespace Stocks.Core.DividendDisplay;
public class CalendarGenerator : ICalendarGenerator
{
    IDateProvider _dateTimeProvider;

    public CalendarGenerator(IDateProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public IEnumerable<IEnumerable<DisplayDay>> GenerateMonth()
    {
        var today = _dateTimeProvider.GetToday();

        var startDay = DayOfWeek.Monday;
        var dayAdjustment = 0;

        var monthsFirstDay = new DateTime(today.Year, today.Month, 1);
        var displayCalendarsFirstMonday = monthsFirstDay;

        if (displayCalendarsFirstMonday.DayOfWeek != startDay)
        {
            dayAdjustment = (int)displayCalendarsFirstMonday.DayOfWeek;
            displayCalendarsFirstMonday = displayCalendarsFirstMonday.AddDays(-dayAdjustment);
        }
        var wholeMonth = Enumerable.Range(0, DateTime.DaysInMonth(monthsFirstDay.Year, monthsFirstDay.Month) + dayAdjustment).Select(i => displayCalendarsFirstMonday.AddDays(i));

        var mondays = wholeMonth.Where(d => d.DayOfWeek == startDay);

        var month = mondays.Select(d => Enumerable.Range(0, 7).Select(i => d.AddDays(i)));

        return month;
    }
}
