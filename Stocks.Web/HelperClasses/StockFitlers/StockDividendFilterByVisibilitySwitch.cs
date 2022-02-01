using Stocks.Domain.Models;
using Stocks.Web.Pages;
using System;
using System.Linq;

namespace Stocks.Web.HelperClasses.StockFitlers
{
    public class StockDividendFilterByVisibilitySwitch : StockDividendFilterBase
    {
        private readonly string _visibilitySwitch;

        public StockDividendFilterByVisibilitySwitch(string visibilitySwitch = null)
        {
            _visibilitySwitch = visibilitySwitch;
        }

        internal override bool ShouldSkip()
        => string.IsNullOrEmpty(_visibilitySwitch);

        internal override bool ShouldFilter(StockDividend sd)
        => _visibilitySwitch switch
        {
            var vs when vs == Common.SwitchToUpcoming => IsUpcoming(sd),
            var vs when vs == Common.SwitchToGraterThan1 => IsRatioGraterThan1(sd),
            var vs when vs == Common.HasSpecial => HasSpecial(sd),
            var vs when vs == Common.HighestRatioPerYear => IsHighestRatioPerYear(sd),
            _ => true
        };

        private static bool IsHighestRatioPerYear(StockDividend sd)
        => sd.CurrentYearsHistory().Sum(x => x.Amount).Round() / sd.Price > 0.08;

        internal static bool IsUpcoming(StockDividend stockDividend)
        {
            var whenToBuyToToday = CalculateWhenToBuyToToday(stockDividend);
            return whenToBuyToToday > Common.ZeroDays && whenToBuyToToday <= Common.TwoWeeks;
        }
        private static int CalculateWhenToBuyToToday(StockDividend stockDividend)
            => (stockDividend.LatestDividendHistory.WhenToBuy - DateTime.Today).Days;

        internal static bool IsRatioGraterThan1(StockDividend stockDividend)
            => stockDividend.DividendToPrice >= Common.OnePercent;

        internal static bool HasSpecial(StockDividend stockDividend)
            => stockDividend.HasSpecial;
    }
}
