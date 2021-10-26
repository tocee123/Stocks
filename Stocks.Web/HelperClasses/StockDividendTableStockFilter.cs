using Stocks.Core.Models;
using System;
using System.Linq;

namespace Stocks.Web.Pages
{
    public record StockDividendTableStockFilter(string TickerFilter, string VisibilitySwitch)
    {
        public bool ShouldDisplay(StockDividend stockDividend)
        {
            var functionsToCheck = new Func<StockDividend, bool>[]
            {
                FilterByTicker,
                (sd)=>string.IsNullOrEmpty(VisibilitySwitch)
            };
            var functionsToCheck1 = new Func<StockDividend, bool>[]
            {
                IsUpcoming,
                IsRatioGraterThan1,
                HasSpecial
            };
            return functionsToCheck.All(f => f(stockDividend)) || functionsToCheck1.Any(f => f(stockDividend));
        }

        internal bool FilterByTicker(StockDividend stockDividend)
        => string.IsNullOrEmpty(TickerFilter)
            || (!string.IsNullOrEmpty(TickerFilter) && stockDividend.Ticker.ToLower().Contains(TickerFilter.ToLower()));

        internal bool IsUpcoming(StockDividend stockDividend)
        {
            var whenToBuyToToday = CalculateWhenToBuyToToday(stockDividend);
            return VisibilitySwitch == Common.SwitchToUpcoming && (whenToBuyToToday > Common.ZeroDays && whenToBuyToToday <= Common.TwoWeeks);
        }
        private static int CalculateWhenToBuyToToday(StockDividend stockDividend)
            => (stockDividend.LatestDividendHistory.WhenToBuy - DateTime.Today).Days;

        internal bool IsRatioGraterThan1(StockDividend stockDividend)
            => VisibilitySwitch == Common.SwitchToGraterThan1 && stockDividend.DividendToPrice >= Common.OnePercent;

        internal bool HasSpecial(StockDividend stockDividend)
            => VisibilitySwitch == Common.HasSpecial && stockDividend.HasSpecial;
    }
}
