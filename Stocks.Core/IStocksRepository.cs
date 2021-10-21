using Stocks.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stocks.Core
{
    public interface IStocksRepository
    {
        Task<IEnumerable<StockDividend>> GetStocks();
    }
}