namespace Stocks.Core.DividendDisplay;

public record DisplayDividendHistory(DateTime Date, string Ticker, string Name, string Css, double Amount)
{
    public static DisplayDividendHistory[] ToDisplayDividendHistories(StockDividend stock, DividendHistory dividendHistory)
    {
        var ex = new DisplayDividendHistory(dividendHistory.ExDate, stock.Ticker, stock.Name.Substring(0, stock.Name.IndexOf("(")), "exDate", dividendHistory.Amount);
        var pay = ex with { Date = dividendHistory.PayDate, Css = "payDate" };
        return new[] { ex, pay };
    }
}