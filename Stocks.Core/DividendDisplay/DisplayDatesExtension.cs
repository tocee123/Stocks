namespace Stocks.Core.DividendDisplay;

public static class DisplayDatesExtension
{
    public static double GetMaximumYield(this IEnumerable<IEnumerable<DisplayDay>> month)
    {
        return month.SelectMany(m => m.SelectMany(x => x.DisplayDividendHistories)).Max(m => m.Yield) * 100;
    }
}