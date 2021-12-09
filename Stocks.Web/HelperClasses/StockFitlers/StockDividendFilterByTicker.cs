using Stocks.Domain.Models;

namespace Stocks.Web.HelperClasses.StockFitlers
{
    public class StockDividendFilterByTicker : StockDividendFilterBase
    {
        private readonly string _tickerFilter;

        public StockDividendFilterByTicker(string tickerFilter = null)
        {
            _tickerFilter = tickerFilter;
        }

        internal override bool ShouldFilter(StockDividend sd)
        => sd.Ticker.ToLower().Contains(_tickerFilter?.ToLower() ?? "");

        internal override bool ShouldSkip()
        => string.IsNullOrEmpty(_tickerFilter);
    }
}
