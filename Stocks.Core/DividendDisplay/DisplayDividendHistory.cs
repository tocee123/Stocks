using Microsoft.Extensions.Primitives;
using Stocks.Domain.Helpers;

namespace Stocks.Core.DividendDisplay;

public record DisplayDividendHistory(DateTime Date, string Ticker, string Name, string Css, double Amount, IEnumerable<DisplayDividendHistoryDetail> Details)
{
    public static DisplayDividendHistory[] ToDisplayDividendHistories(StockDividend stock, DividendHistory dividendHistory)
    {
        var details = new List<DisplayDividendHistoryDetail> { 
        new("Current price:", stock.Price.ToDollarDisplay()),
        new("Current amount:", dividendHistory.Amount.ToDollarDisplay()),
        new("Dividend to price:", (dividendHistory.Amount/stock.Price).ToPercentageDisplay()),
        new("Dividend yield:", (stock.DividendHistories.Where(dh=>dh.PayDate.Year == dividendHistory.PayDate.Year).Sum(dh=>dh.Amount)/stock.Price).ToPercentageDisplay()),
        };

        var stockName = stock.Name.Contains("(")
            ? stock.Name.Substring(0, stock.Name.IndexOf("("))
            : stock.Name;

        var ex = new DisplayDividendHistory(dividendHistory.ExDate, stock.Ticker, stockName, "exDate", dividendHistory.Amount, details);
        var pay = ex with { Date = dividendHistory.PayDate, Css = "payDate" };
        return new[] { ex, pay };
    }
}
