using Stocks.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stocks.Core
{
    public interface IStocksLoader
    {
        Task<IEnumerable<StockDividend>> GetStockDividendsAsync(string[] stockShortNames);
    }
}