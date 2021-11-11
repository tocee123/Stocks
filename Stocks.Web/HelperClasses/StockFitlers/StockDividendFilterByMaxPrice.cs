using Stocks.Core.Models;
using System;
using System.Collections.Generic;

namespace Stocks.Web.HelperClasses.StockFitlers
{
    public class StockDividendFilterByMaxPrice : IStockDividendFilter
    {
        private readonly int _maxPrice;

        public StockDividendFilterByMaxPrice(int maxPrice = 0)
        {
            _maxPrice = maxPrice;
        }

        public IEnumerable<Func<StockDividend, bool>> GetFilterArray()
        => new Func<StockDividend, bool>[]
        {
            _maxPrice == 0 ? st => true : FilterByMaxPrice
        };

        internal bool FilterByMaxPrice(StockDividend sd)
         => _maxPrice == 0 || sd.Price <= _maxPrice;
    }
}
