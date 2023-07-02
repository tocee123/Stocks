namespace Stocks.Core.Loaders
{
    public class StocksLoader : IStocksLoader
    {
        private readonly IStockDividendHistoryLoader _stockDividendHistoryLoader;

        public StocksLoader(IStockDividendHistoryLoader stockDividendHistoryLoader)
        {
            _stockDividendHistoryLoader = stockDividendHistoryLoader;
        }

        public async Task<IEnumerable<StockDividend>> GetStockDividendsAsync(IEnumerable<string> tickers)
        {
            var stocksToCheck = tickers.Distinct()
            .Select(_stockDividendHistoryLoader.DownloadStockHistoryAsync);
            var stocks = await Task.WhenAll(stocksToCheck);
            var orderedStocks = stocks.Where(s => s.IsCorrectlyDownloaded && s.LatestDividendHistory is not null).OrderByDescending(s => s.LatestDividendHistory.ExDate);
            return orderedStocks;
        }
    }
}
