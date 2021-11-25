using Stocks.Core;
using Stocks.Core.Models;
using System;

namespace Stocks.Web.Pages
{
    public record StockDividendDisplay(string Name, string Ticker, double Price, string Exdate, string RecordDate, string PayDate, string DeclarationDate, DateTime WhenToBuy, string WhenToBuyDisplay, DateTime WhenToSell, string WhenToSellDisplay, double Amount, double DividendToPrice, string DividendToPriceDisplay, bool HasSpecial)
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
            DateCalculator.CalculateWhenToBuy(sd.LatestDividendHistory.ExDate),
            DateCalculator.CalculateWhenToBuy(sd.LatestDividendHistory.ExDate).ToYyyyMmDd(),
            DateCalculator.CalculateWhenToSell(sd.LatestDividendHistory.RecordDate),
            DateCalculator.CalculateWhenToSell(sd.LatestDividendHistory.RecordDate).ToYyyyMmDd(),
            Math.Round(sd.LatestDividendHistory.Amount, 2),
            sd.DividendToPrice,
            sd.DividendToPrice.ToPercentageDisplay(),
            sd.HasSpecial);
    }
}
