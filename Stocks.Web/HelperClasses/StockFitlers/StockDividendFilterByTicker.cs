using Stocks.Core.Models;

namespace Stocks.Web.HelperClasses.StockFitlers
{
    public class StockDividendFilterByTicker : IStockDividendFilter
    {
        private readonly string _tickerFilter;

        public StockDividendFilterByTicker(string tickerFilter = null)
        {
            _tickerFilter = tickerFilter;
        }

        public bool Filter(StockDividend sd)
        => string.IsNullOrEmpty(_tickerFilter) || FilterByTicker(sd);

        internal bool FilterByTicker(StockDividend sd)
        => sd.Ticker.ToLower().Contains(_tickerFilter?.ToLower() ?? "");
    }
}
