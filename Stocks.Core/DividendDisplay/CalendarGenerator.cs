using Stocks.Core.Repositories;

namespace Stocks.Core.DividendDisplay;
public class CalendarGenerator : ICalendarGenerator
{
    readonly IDateProvider _dateTimeProvider;
    readonly IStocksRepository _stocksRepository;

    public CalendarGenerator(IDateProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public DateTime Today => _dateTimeProvider.GetToday();

    public async Task<IEnumerable<IEnumerable<DisplayDay>>> GenerateMonthAsync()
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

        var stockDividends = await _stocksRepository.GetStocksAsync();

        var dividendHistories = stockDividends.SelectMany(sd => sd.DividendHistories, (sd, dh) => new { sd.Ticker, DividendHistory = dh })
            .Where(x => IsDvividendHistoryInCurrentMonth(x.DividendHistory));

        var mondays = wholeMonth.Where(d => d.DayOfWeek == startDay);

        var month = mondays.Select(d => Enumerable.Range(0, 7).Select(i => d.AddDays(i))
        .Select(d => new DisplayDay(d.Day, GetDisplayDateOfWeek(d), GetClassForCard(d), GetClassForHeader(d))));

        return month;
    }

    private bool IsDvividendHistoryInCurrentMonth(DividendHistory dividendHistory)
        => dividendHistory.ExDate >= _dateTimeProvider.GetFirstDayOfCurrentMonth()
        || dividendHistory.ExDate <= _dateTimeProvider.GetLastDayOfCurrentMonth()
        || dividendHistory.PayDate >= _dateTimeProvider.GetFirstDayOfCurrentMonth()
        || dividendHistory.PayDate <= _dateTimeProvider.GetLastDayOfCurrentMonth();

    private static string GetDisplayDateOfWeek(DateTime day)
        => day.DayOfWeek.ToString()[..3];

    private string GetClassForHeader(DateTime date) => date switch
    {
        { } when date == _dateTimeProvider.GetToday() => "headerToday",
        { } when date.Month != _dateTimeProvider.GetToday().Month => "headerOtherMonth",
        { } when _dateTimeProvider.IsWeekend(date) => "headerWeekend",
        _ => "header",
    };

    private string GetClassForCard(DateTime date)
    => _dateTimeProvider.IsWeekend(date) ? "cardWeekend" : "card";
}
