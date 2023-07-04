using Stocks.Core.Repositories;

namespace Stocks.Core.DividendDisplay;
public class CalendarGenerator : ICalendarGenerator
{
    readonly IDateProvider _dateTimeProvider;
    readonly IStocksRepository _stocksRepository;
    readonly DayOfWeek _startDay = DayOfWeek.Monday;

    public CalendarGenerator(IDateProvider dateTimeProvider, IStocksRepository stocksRepository)
    {
        _dateTimeProvider = dateTimeProvider;
        _stocksRepository = stocksRepository;
    }

    public DateTime Today => _dateTimeProvider.GetToday();

    public async Task<IEnumerable<IEnumerable<DisplayDay>>> GenerateMonthAsync()
    {
        var dividendHistories = await GetDividendHistoriesByDays();
        var weeksFirstDays = GetFirstdayOfTheWeekForTheCurrentMonth();
        var weeks = GenerateWeeks(weeksFirstDays, dividendHistories);

        return weeks;
    }

    private IEnumerable<IEnumerable<DisplayDay>> GenerateWeeks(IEnumerable<DateTime> weeksFirstDays, Dictionary<DateTime, List<DisplayDividendHistory>> dividendHistories)
    => weeksFirstDays.Select(d => Enumerable.Range(0, 7).Select(i => d.AddDays(i))
        .Select(d => DisplayDay.ToDisplayDay(_dateTimeProvider, d, dividendHistories)));

    private async Task<Dictionary<DateTime, List<DisplayDividendHistory>>> GetDividendHistoriesByDays()
    {
        var stockDividends = await _stocksRepository.GetStocksAsync();

        var dividendHistories = stockDividends.SelectMany(sd => sd.DividendHistories, (sd, dh) => new { sd.Ticker, DividendHistory = dh })
            .Where(x => IsDvividendHistoryInCurrentMonth(x.DividendHistory))
            .SelectMany(x => DisplayDividendHistory.ToDisplayDividendHistories(x.Ticker, x.DividendHistory))
            .GroupBy(x => x.Date)
            .ToDictionary(x => x.Key, x => x.OrderBy(x => x.Ticker).ToList());
        return dividendHistories;
    }

    private IEnumerable<DateTime> GetFirstdayOfTheWeekForTheCurrentMonth()
    {
        var today = _dateTimeProvider.GetToday();
        var displayCalendarsFirstMonday = GetWeeksMonday(today);

        return Enumerable.Range(0, DateTime.DaysInMonth(today.Year, today.Month)-today.Day+1).Select(i => today.AddDays(i))
            .Where(d => d.DayOfWeek == _startDay);
    }

    private DateTime GetWeeksMonday(DateTime date)
    {
        var displayCalendarsFirstMonday = date;
        if (date.DayOfWeek != _startDay)
        {
            var adjustment = (int)date.DayOfWeek - 1;
            displayCalendarsFirstMonday = displayCalendarsFirstMonday.AddDays(-adjustment);
        }
        return displayCalendarsFirstMonday;
    }

    private bool IsDvividendHistoryInCurrentMonth(DividendHistory dividendHistory)
        => dividendHistory.ExDate >= _dateTimeProvider.GetFirstDayOfCurrentMonth()
        || dividendHistory.ExDate <= _dateTimeProvider.GetLastDayOfCurrentMonth()
        || dividendHistory.PayDate >= _dateTimeProvider.GetFirstDayOfCurrentMonth()
        || dividendHistory.PayDate <= _dateTimeProvider.GetLastDayOfCurrentMonth();
}
