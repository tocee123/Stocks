using Stocks.Domain.Models;
using System.Linq;

namespace Stocks.Web.HelperClasses.StockFitlers
{
    public class StockDividendTableStockFilter
    {
        private readonly IStockDividendFilter[] _filters;

        public StockDividendTableStockFilter(string tickerFilter, string visibilitySwitch, int maxPrice)
        {
            _filters = new IStockDividendFilter[] {
                new StockDividendFilterByTicker(tickerFilter),
                new StockDividendFilterByVisibilitySwitch(visibilitySwitch),
                new StockDividendFilterByMaxPrice(maxPrice)
            };
        }

        public bool ShouldDisplay(StockDividend stockDividend)
        => _filters.All(filter => filter.Filter(stockDividend));
    }
}
