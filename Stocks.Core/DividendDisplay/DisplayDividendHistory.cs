using Stocks.Domain.Helpers;

namespace Stocks.Core.DividendDisplay;

public record DisplayDividendHistory(DateTime Date, string Ticker, string Name, string Css, double Amount, double Yield, string InfoText, EventType EventType, IEnumerable<DisplayDividendHistoryDetail> Details)
{
    public static DisplayDividendHistory[] ToDisplayDividendHistories(StockDividend stock, DividendHistory dividendHistory)
    {
        double yield = stock.DividendHistories.Where(dh => dh.PayDate.Year == dividendHistory.PayDate.Year).Sum(dh => dh.Amount) / stock.Price;
        var details = new List<DisplayDividendHistoryDetail> {
        new("Current price:", stock.Price.ToDollarDisplay()),
        new("Current amount:", dividendHistory.Amount.ToDollarDisplay()),
        new("Dividend to price:", (dividendHistory.Amount/stock.Price).ToPercentageDisplay()),
        new("Dividend yield:", yield.ToPercentageDisplay()),
        };

        var stockName = stock.Name.RemoveTicker().ShrinkNAme();
        var ex = new DisplayDividendHistory(dividendHistory.ExDate, stock.Ticker, stockName, "exDate", dividendHistory.Amount, yield, "Ex-Dividend", EventType.ExDividend, details);
        var pay = ex with { EventType = EventType.PayDate, Date = dividendHistory.PayDate, Css = "payDate", InfoText = "Pay date" };
        return new[] { ex, pay };
    }
}

public enum EventType
{
    ExDividend,
    PayDate
}

public static class StockNameExtensions
{
    public static string RemoveTicker(this string name)
    {
        var characterToIndex = "(";

        return name.Contains(characterToIndex)
            ? name[..name.IndexOf(characterToIndex)]
            : name;
    }

    public static string ShrinkNAme(this string name)
    {
        var stockNameMaxLength = 30;
        return name.Length > stockNameMaxLength
            ? $"{name[..stockNameMaxLength]}..."
            : name;
    }
}
