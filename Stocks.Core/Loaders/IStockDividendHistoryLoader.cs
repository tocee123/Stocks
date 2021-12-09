using Stocks.Domain.Models;
using System.Threading.Tasks;

namespace Stocks.Core.Loaders
{
    public interface IStockDividendHistoryLoader
    {
        Task<StockDividend> DownloadStockHistoryAsync(string ticker);
    }
}