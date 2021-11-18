using Stocks.Core.Models;

namespace Stocks.Web.HelperClasses.StockFitlers
{
    public abstract class StockDividendFilterBase : IStockDividendFilter
    {
        public bool Filter(StockDividend sd)
        => ShouldSkip() || ShouldFilter(sd);

        internal abstract bool ShouldSkip();
        internal abstract bool ShouldFilter(StockDividend sd);
    }
}
