using Stocks.Core.Models;
using System;
using System.Linq;

namespace Stocks.Web.Pages
{
    public class StockDividendTableStockFilter
    {
        private readonly string _tickerFilter;
        private readonly string _visibilitySwitch;

        public StockDividendTableStockFilter(string tickerFilter, string visibilitySwitch)
        {
            this._tickerFilter = tickerFilter;
            this._visibilitySwitch = visibilitySwitch;
        }

        public bool ShouldDisplay(StockDividend stockDividend)
        {
            var functionsToCheck = new Func<StockDividend, bool>[]
            {
                FilterByTicker,
                (sd)=>string.IsNullOrEmpty(_visibilitySwitch)
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
        => string.IsNullOrEmpty(_tickerFilter)
            || (!string.IsNullOrEmpty(_tickerFilter) && stockDividend.Ticker.ToLower().Contains(_tickerFilter.ToLower()));

        internal bool IsUpcoming(StockDividend stockDividend)
        {
            var whenToBuyToToday = CalculateWhenToBuyToToday(stockDividend);
            return _visibilitySwitch == Common.SwitchToUpcoming && (whenToBuyToToday > Common.ZeroDays && whenToBuyToToday <= Common.TwoWeeks);
        }
        private static int CalculateWhenToBuyToToday(StockDividend stockDividend)
            => (stockDividend.LatestDividendHistory.WhenToBuy - DateTime.Today).Days;

        internal bool IsRatioGraterThan1(StockDividend stockDividend)
            => _visibilitySwitch == Common.SwitchToGraterThan1 && stockDividend.DividendToPrice >= Common.OnePercent;

        internal bool HasSpecial(StockDividend stockDividend)
            => _visibilitySwitch == Common.HasSpecial && stockDividend.HasSpecial;
    }
}
