using Stocks.Core.Models;
using Stocks.Web.Pages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stocks.Web.HelperClasses.StockFitlers
{
    public class StockDividendTableStockFilter
    {
        private readonly string _tickerFilter;
        private readonly string _visibilitySwitch;
        private readonly int _maxPrice;

        public StockDividendTableStockFilter(string tickerFilter, string visibilitySwitch, int maxPrice)
        {
            _tickerFilter = tickerFilter;
            _visibilitySwitch = visibilitySwitch;
            _maxPrice = maxPrice;
        }

        public bool ShouldDisplay(StockDividend stockDividend)
        {
            var functionsToCheck = new List<Func<StockDividend, bool>>();
            AddTicketFilter(functionsToCheck);
            AddMaxPriceFilter(functionsToCheck);
            AddFilterByVisibility(functionsToCheck);
            AddDefaultFilter(functionsToCheck);

            return functionsToCheck.Any(f => f(stockDividend));
        }

        private void AddTicketFilter(List<Func<StockDividend, bool>> functionsToCheck)
        {
            if (!string.IsNullOrEmpty(_tickerFilter))
                functionsToCheck.Add(FilterByTicker);
        }

        private void AddMaxPriceFilter(List<Func<StockDividend, bool>> functionsToCheck)
        {
            if (_maxPrice > 0)
                functionsToCheck.Add(FilterByMaxPrice);
        }

        private void AddFilterByVisibility(List<Func<StockDividend, bool>> functionsToCheck)
        {
            if (!string.IsNullOrEmpty(_visibilitySwitch))
                functionsToCheck.AddRange(new Func<StockDividend, bool>[]
                {
                    IsUpcoming,
                    IsRatioGraterThan1,
                    HasSpecial
                });
        }

        private static void AddDefaultFilter(List<Func<StockDividend, bool>> functionsToCheck)
        {
            if (!functionsToCheck.Any())
                functionsToCheck.Add(st => true);
        }

        internal bool FilterByTicker(StockDividend stockDividend)
        => stockDividend.Ticker.ToLower().Contains(_tickerFilter?.ToLower() ?? "");

        internal bool FilterByMaxPrice(StockDividend stockDividend)
        => _maxPrice == 0 || stockDividend.Price <= _maxPrice;

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
