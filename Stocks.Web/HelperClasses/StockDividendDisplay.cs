using Stocks.Core.Models;
using System;

namespace Stocks.Web.Pages
{
    public record StockDividendDisplay(string Name, string Ticker, double Price, string LatestExdate, string LatestRecordDate, string LatestPayDate, string LatestDeclarationDate, DateTime LatestWhenToBuy, string LatestWhenToBuyDisplay, double Amount, double DividendToPrice, string DividendToPriceDisplay, bool HasSpecial)
    {
        public static StockDividendDisplay Map(StockDividend sd)
        => new StockDividendDisplay(
            sd.Name,
            sd.Ticker,
            sd.Price,
            sd.LatestDividendHistory.ExDate.ToYyyyMmDd(),
            sd.LatestDividendHistory.RecordDate.ToYyyyMmDd(),
            sd.LatestDividendHistory.PayDate.ToYyyyMmDd(),
            sd.LatestDividendHistory.DeclarationDate.ToYyyyMmDd(),
            sd.LatestDividendHistory.WhenToBuy,
            sd.LatestDividendHistory.WhenToBuy.ToYyyyMmDd(),
            sd.LatestDividendHistory.Amount,
            sd.DividendToPrice,
            sd.DividendToPrice.ToPercentageDisplay(),
            sd.HasSpecial);
    }
}
