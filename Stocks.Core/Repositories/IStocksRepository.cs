using Stocks.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stocks.Core.Repositories
{
    public interface IStocksRepository
    {
        Task<IEnumerable<StockDividend>> GetStocksAsync();
    }
}