using Stocks.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stocks.Core.Cache
{
    public interface ICachedRepositoryManager
    {
        Task<IEnumerable<string>> GetStocksOfInterestAsync();
        Task<IEnumerable<StockDividend>> GetStockDividendsAsync();
    }
}