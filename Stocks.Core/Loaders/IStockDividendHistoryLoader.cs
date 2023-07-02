namespace Stocks.Core.Loaders
{
    public interface IStockDividendHistoryLoader
    {
        Task<StockDividend> DownloadStockHistoryAsync(string ticker);
    }
}