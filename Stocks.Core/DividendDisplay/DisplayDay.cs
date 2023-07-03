namespace Stocks.Core.DividendDisplay;
public record DisplayDay(int Day, string DayOfWeek, string CardCss, string HeaderCss, IEnumerable<DisplayDividendHistory> DisplayDividendHistories)
{
    public static DisplayDay ToDisplayDay(IDateProvider dateProvider, DateTime d, Dictionary<DateTime, List<DisplayDividendHistory>> dividendHistoriesByDate)
    => new(d.Day, GetDisplayDateOfWeek(d), GetClassForCard(dateProvider, d), GetClassForHeader(dateProvider, d), dividendHistoriesByDate.GetValueOrDefault(d, new List<DisplayDividendHistory>()));

    private static string GetDisplayDateOfWeek(DateTime day)
        => day.DayOfWeek.ToString()[..3];

    private static string GetClassForHeader(IDateProvider dateProvider, DateTime date) => date switch
    {
        { } when date == dateProvider.GetToday() => "headerToday",
        { } when date.Month != dateProvider.GetToday().Month => "headerOtherMonth",
        { } when dateProvider.IsWeekend(date) => "headerWeekend",
        _ => "header",
    };

    private static string GetClassForCard(IDateProvider dateProvider, DateTime date)
    => dateProvider.IsWeekend(date) ? "cardWeekend" : "card";
}
