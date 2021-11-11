﻿using Stocks.Core.Models;
using System;
using System.Collections.Generic;

namespace Stocks.Web.HelperClasses.StockFitlers
{
    public class StockDividendFilterByTicket : IStockDividendFilter
    {
        private readonly string _tickerFilter;

        public StockDividendFilterByTicket(string tickerFilter = null)
        {
            _tickerFilter = tickerFilter;
        }

        public IEnumerable<Func<StockDividend, bool>> GetFilterArray()
        => new Func<StockDividend, bool>[]
        {
            string.IsNullOrEmpty(_tickerFilter) ? st => true : FilterByTicker
        };

        internal bool FilterByTicker(StockDividend sd)
        => sd.Ticker.ToLower().Contains(_tickerFilter?.ToLower() ?? "");
    }
}
