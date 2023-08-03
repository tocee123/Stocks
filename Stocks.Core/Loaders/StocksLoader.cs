using Microsoft.Extensions.Logging;

namespace Stocks.Core.Loaders
{
    public class StocksLoader : IStocksLoader
    {
        private readonly IStockDividendHistoryLoader _stockDividendHistoryLoader;
        private readonly ILogger<StocksLoader> _logger;

        public StocksLoader(ILogger<StocksLoader> logger, IStockDividendHistoryLoader stockDividendHistoryLoader)
        {
            _stockDividendHistoryLoader = stockDividendHistoryLoader;
            _logger = logger;
        }

        public async Task<IEnumerable<StockDividend>> GetStockDividendsAsync(IEnumerable<string> tickers)
        {
            _logger.LogInformation($"Processing {tickers.Count()} tickers");

            var stocksToCheck = tickers.Distinct()
            .Select(_stockDividendHistoryLoader.DownloadStockHistoryAsync);

            var stocks = await Task.WhenAll(stocksToCheck);
            var orderedStocks = stocks.Where(s => s.IsCorrectlyDownloaded && s.LatestDividendHistory is not null).OrderByDescending(s => s.LatestDividendHistory.ExDate);

            return orderedStocks;
        }
    }
}
