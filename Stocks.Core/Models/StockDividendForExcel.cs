using System;
using System.Linq;

namespace Stocks.Core.Models
{
    public record StockDividendForExcel(string Name, string Ticker, double Price, DateTime ExDate, DateTime RecordDate, DateTime DeclarationDate, DateTime PayDate, DateTime WhenToBuy, DateTime WhenToSell, double Amount, double DividendToPrice)
    {
        public static StockDividendForExcel Map(StockDividend sd)
            => new StockDividendForExcel(
                sd.Name,
                sd.Ticker,
                sd.Price,
                sd.LatestDividendHistory.ExDate,
                sd.LatestDividendHistory.RecordDate,
                sd.LatestDividendHistory.DeclarationDate,
                sd.LatestDividendHistory.PayDate,
                DateCalculator.CalculateWhenToBuy(sd.LatestDividendHistory.ExDate),
                DateCalculator.CalculateWhenToSell(sd.LatestDividendHistory.RecordDate),
                sd.LatestDividendHistory.Amount,
                sd.DividendToPrice
                );

        public static string[] GetPropertyNames()
            => typeof(StockDividendForExcel).GetProperties().Select(p => p.Name).ToArray();
    }
}
