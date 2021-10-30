using Stocks.Core.Cache;
using Stocks.Core.Extensions;
using Stocks.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stocks.Core
{
    public class StocksRepository : IStocksRepository
    {
        private readonly IStocksLoader _stocksLoader;
        private readonly ICachedRepositoryManager _cachedRepositoryManager;

        public StocksRepository(IStocksLoader stocksLoader, ICachedRepositoryManager cachedRepositoryManager)
        {
            _stocksLoader = stocksLoader;
            _cachedRepositoryManager = cachedRepositoryManager;
        }

        public async Task<IEnumerable<StockDividend>> GetStocks()
        {
            var stockDividends = await _cachedRepositoryManager.GetStockDividendsAsync();
            if (!stockDividends.AnyWhenNull())
            {
                var stockOfInterestTickers = await _cachedRepositoryManager.GetStocksOfInterestAsync();
                stockDividends = await _stocksLoader.GetStockDividendsAsync(stockOfInterestTickers);
                await _cachedRepositoryManager.SaveStockDividendsAsync(stockDividends);
            }
            return stockDividends;
        }
    }
}
