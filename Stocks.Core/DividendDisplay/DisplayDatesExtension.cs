namespace Stocks.Core.DividendDisplay;

public static class DisplayDatesExtension
{
    public static double GetMaximumYield(this IEnumerable<IEnumerable<DisplayDay>> month)
    => month.SelectMany(m => m.SelectMany(x => x.DisplayDividendHistories)).OrderByDescending(x => x.Yield).Skip(1).Max(m => m.Yield) * 100;
}