namespace Stocks.Core.Loaders
{
    public class StockDividendHistoryEmptyLoader : IStockDividendHistoryLoader
    {
        public async Task<StockDividend> DownloadStockHistoryAsync(string ticker)
        {
            return new StockDividend { IsCorrectlyDownloaded = true, Price = 5, Name = "Tony Stock", Ticker = "TSK", DividendHistories = new[] { new DividendHistory { Amount = 10, DeclarationDate = DateTime.Now, ExDate = DateTime.Now, PayDate = DateTime.Now, RecordDate = DateTime.Now, Type = "test" } } };
        }
    }
}
