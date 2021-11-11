using Stocks.Core.Models;

namespace Stocks.Web.HelperClasses.StockFitlers
{
    public interface IStockDividendFilter
    {
        bool Filter(StockDividend sd);
    }
}
