using Stocks.Domain.Helpers;

namespace Stocks.Core.DividendDisplay;

public record DisplayDividendHistory(DateTime Date, string Ticker, string Name, string Css, double Amount, double Yield, IEnumerable<DisplayDividendHistoryDetail> Details)
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

        var characterToIndex = "(";

        var stockName = stock.Name.Contains(characterToIndex)
            ? stock.Name[..stock.Name.IndexOf(characterToIndex)]
            : stock.Name;

        var ex = new DisplayDividendHistory(dividendHistory.ExDate, stock.Ticker, stockName, "exDate", dividendHistory.Amount, yield, details);
        var pay = ex with { Date = dividendHistory.PayDate, Css = "payDate" };
        return new[] { ex, pay };
    }
}
