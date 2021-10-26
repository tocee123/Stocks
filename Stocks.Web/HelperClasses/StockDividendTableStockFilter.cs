using Stocks.Core.Models;
using System;
using System.Linq;

namespace Stocks.Web.Pages
{
    public record StockDividendTableStockFilter(string ShortNameFilter, string VisibilitySwitch)
    {
        public bool ShouldDisplay(StockDividend stockDividend)
        {
            var functionsToCheck = new Func<StockDividend, bool>[]
            {
                FilterByShortName,
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

        private bool FilterByShortName(StockDividend stockDividend)
        => string.IsNullOrEmpty(ShortNameFilter)
            || (!string.IsNullOrEmpty(ShortNameFilter) && stockDividend.ShortName.ToLower().Contains(ShortNameFilter.ToLower()));

        public bool IsUpcoming(StockDividend stockDividend)
        {
            var whenToBuyToToday = CalculateWhenToBuyToToday(stockDividend);
            return VisibilitySwitch == Common.SwitchToUpcoming && (whenToBuyToToday >= Common.ZeroDays && whenToBuyToToday <= Common.TwoWeeks);
        }
        private int CalculateWhenToBuyToToday(StockDividend stockDividend)
            => (stockDividend.LatestDividendHistory.WhenToBuy - DateTime.Today).Days;

        public bool IsRatioGraterThan1(StockDividend stockDividend)
            => VisibilitySwitch == Common.SwitchToGraterThan1 && stockDividend.DividendToPrice >= Common.OnePercent;

        private bool HasSpecial(StockDividend stockDividend)
            => VisibilitySwitch == Common.HasSpecial && stockDividend.HasSpecial;
    }
}
