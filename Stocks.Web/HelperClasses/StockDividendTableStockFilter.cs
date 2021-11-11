﻿using Stocks.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stocks.Web.Pages
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

            if (!string.IsNullOrEmpty(_tickerFilter))
                functionsToCheck.Add(FilterByTicker);

            if (_maxPrice > 0)
                functionsToCheck.Add(FilterByMaxPrice);

            if (!string.IsNullOrEmpty(_visibilitySwitch))
                functionsToCheck.AddRange(new Func<StockDividend, bool>[]
                {
                    IsUpcoming,
                    IsRatioGraterThan1,
                    HasSpecial
                });

            if (!functionsToCheck.Any())
                functionsToCheck.Add(st => true);

            return functionsToCheck.Any(f => f(stockDividend));
        }

        internal bool FilterByTicker(StockDividend stockDividend)
        => stockDividend.Ticker.ToLower().Contains(_tickerFilter?.ToLower()??"");

        internal bool FilterByMaxPrice(StockDividend stockDividend)
        => _maxPrice == 0 || stockDividend.Price <= _maxPrice;

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
