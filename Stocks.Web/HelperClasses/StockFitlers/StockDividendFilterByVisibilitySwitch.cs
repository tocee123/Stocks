using Stocks.Core.Models;
using Stocks.Web.Pages;
using System;
using System.Collections.Generic;

namespace Stocks.Web.HelperClasses.StockFitlers
{
    public class StockDividendFilterByVisibilitySwitch : IStockDividendFilter
    {
        private readonly string _visibilitySwitch;

        public StockDividendFilterByVisibilitySwitch(string visibilitySwitch = null)
        {
            _visibilitySwitch = visibilitySwitch;
        }

        public IEnumerable<Func<StockDividend, bool>> GetFilterArray()
        {
            var result = new List<Func<StockDividend, bool>>();

            Func<StockDividend, bool> function = _visibilitySwitch switch
            {
                var vs when vs == Common.SwitchToUpcoming => IsUpcoming,
                var vs when vs == Common.SwitchToGraterThan1 => IsRatioGraterThan1,
                var vs when vs == Common.HasSpecial => HasSpecial,
                _ => st => true
            };
            result.Add(function);
            return result;
        }

        internal bool IsUpcoming(StockDividend stockDividend)
        {
            var whenToBuyToToday = CalculateWhenToBuyToToday(stockDividend);
            return _visibilitySwitch == Common.SwitchToUpcoming && whenToBuyToToday > Common.ZeroDays && whenToBuyToToday <= Common.TwoWeeks;
        }
        private static int CalculateWhenToBuyToToday(StockDividend stockDividend)
            => (stockDividend.LatestDividendHistory.WhenToBuy - DateTime.Today).Days;

        internal bool IsRatioGraterThan1(StockDividend stockDividend)
            => _visibilitySwitch == Common.SwitchToGraterThan1 && stockDividend.DividendToPrice >= Common.OnePercent;

        internal bool HasSpecial(StockDividend stockDividend)
            => _visibilitySwitch == Common.HasSpecial && stockDividend.HasSpecial;
    }
}
