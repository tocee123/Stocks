using Stocks.Domain.Helpers;
using Stocks.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stocks.Web.Pages
{
    public record StockDividendDisplay(string Name, string Ticker, double Price, string Exdate, string RecordDate, string PayDate, string DeclarationDate, DateTime WhenToBuy, string WhenToBuyDisplay, DateTime WhenToSell, string WhenToSellDisplay, double Amount, double DividendToPrice, string DividendToPriceDisplay, bool HasSpecial, double CumulatedDividend, int TimesPayedDividends)
    {
        public static StockDividendDisplay Map(StockDividend sd)
        => new (
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
            sd.LatestDividendHistory.Amount.Round(),
            sd.DividendToPrice,
            sd.DividendToPrice.ToPercentageDisplay(),
            sd.HasSpecial,
            sd.CurrentYearsHistory().Sum(x => x.Amount).Round(),
            sd.CurrentYearsHistory().Count());

        public string CumulatedDividendRatio { get => (CumulatedDividend / Price).ToPercentageDisplay(); }
    }

    public static class StockDividendExtensions
    {
        public static IEnumerable<DividendHistory> CurrentYearsHistory(this StockDividend sd)
        => sd.DividendHistories.Where(x => x.ExDate.Date > DateTime.Today.AddDays(-365));
    }
}
