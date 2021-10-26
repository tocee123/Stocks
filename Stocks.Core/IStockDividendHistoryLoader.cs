using Stocks.Core.Models;
using System.Threading.Tasks;

namespace Stocks.Core
{
    public interface IStockDividendHistoryLoader
    {
        Task<StockDividend> DownloadStockHistoryAsync(string ticker);
    }
}