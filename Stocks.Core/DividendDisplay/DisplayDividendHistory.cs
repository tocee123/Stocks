namespace Stocks.Core.DividendDisplay;

public record DisplayDividendHistory(DateTime Date, string Ticker, string Css, double Amount)
{
    public static DisplayDividendHistory[] ToDisplayDividendHistories(string ticker, DividendHistory dividendHistory)
    {
        var ex = new DisplayDividendHistory(dividendHistory.ExDate, ticker, "exDate", dividendHistory.Amount);
        var pay = ex with { Date = dividendHistory.PayDate, Css = "payDate" };
        return new[] { ex, pay };
    }
}