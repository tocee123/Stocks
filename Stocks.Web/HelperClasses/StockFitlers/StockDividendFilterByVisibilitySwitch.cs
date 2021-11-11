using Stocks.Core.Models;
using Stocks.Web.Pages;
using System;

namespace Stocks.Web.HelperClasses.StockFitlers
{
    public class StockDividendFilterByVisibilitySwitch : IStockDividendFilter
    {
        private readonly string _visibilitySwitch;

        public StockDividendFilterByVisibilitySwitch(string visibilitySwitch = null)
        {
            _visibilitySwitch = visibilitySwitch;
        }

        public bool Filter(StockDividend sd)
        => _visibilitySwitch switch
        {
            var vs when vs == Common.SwitchToUpcoming => IsUpcoming(sd),
            var vs when vs == Common.SwitchToGraterThan1 => IsRatioGraterThan1(sd),
            var vs when vs == Common.HasSpecial => HasSpecial(sd),
            _ => true
        };

        internal bool IsUpcoming(StockDividend stockDividend)
        {
            var whenToBuyToToday = CalculateWhenToBuyToToday(stockDividend);
            return whenToBuyToToday > Common.ZeroDays && whenToBuyToToday <= Common.TwoWeeks;
        }
        private static int CalculateWhenToBuyToToday(StockDividend stockDividend)
            => (stockDividend.LatestDividendHistory.WhenToBuy - DateTime.Today).Days;

        internal bool IsRatioGraterThan1(StockDividend stockDividend)
            =>  stockDividend.DividendToPrice >= Common.OnePercent;

        internal bool HasSpecial(StockDividend stockDividend)
            => stockDividend.HasSpecial;
    }
}
