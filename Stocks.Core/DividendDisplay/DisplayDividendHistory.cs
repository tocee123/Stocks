namespace Stocks.Core.DividendDisplay;

public record DisplayDividendHistory(DateTime Date, string Ticker, string Name, string Css, double Amount)
{
    public static DisplayDividendHistory[] ToDisplayDividendHistories(StockDividend stock, DividendHistory dividendHistory)
    {
        var ex = new DisplayDividendHistory(dividendHistory.ExDate, stock.Ticker, stock.Name, "exDate", dividendHistory.Amount);
        var pay = ex with { Date = dividendHistory.PayDate, Css = "payDate" };
        return new[] { ex, pay };
    }
}