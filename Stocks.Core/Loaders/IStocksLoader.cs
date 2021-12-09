using Stocks.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stocks.Core.Loaders
{
    public interface IStocksLoader
    {
        Task<IEnumerable<StockDividend>> GetStockDividendsAsync(IEnumerable<string> tickers);
    }
}