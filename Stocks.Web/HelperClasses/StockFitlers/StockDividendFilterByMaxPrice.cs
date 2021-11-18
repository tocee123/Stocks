using Stocks.Core.Models;

namespace Stocks.Web.HelperClasses.StockFitlers
{
    public class StockDividendFilterByMaxPrice : StockDividendFilterBase
    {
        private readonly int _maxPrice;

        public StockDividendFilterByMaxPrice(int maxPrice = 0)
        {
            _maxPrice = maxPrice;
        }

        internal override bool ShouldFilter(StockDividend sd)
        => sd.Price <= _maxPrice;

        internal override bool ShouldSkip()
        => _maxPrice == 0;         
    }
}
