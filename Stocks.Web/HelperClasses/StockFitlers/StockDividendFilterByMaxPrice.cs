using Stocks.Core.Models;

namespace Stocks.Web.HelperClasses.StockFitlers
{
    public class StockDividendFilterByMaxPrice : IStockDividendFilter
    {
        private readonly int _maxPrice;

        public StockDividendFilterByMaxPrice(int maxPrice = 0)
        {
            _maxPrice = maxPrice;
        }

        public bool Filter(StockDividend sd)
        => _maxPrice == 0 || FilterByMaxPrice(sd);

        internal bool FilterByMaxPrice(StockDividend sd)
         => _maxPrice == 0 || sd.Price <= _maxPrice;
    }
}
