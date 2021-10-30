using Stocks.Core.Cache;
using Stocks.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stocks.Core
{
    public class StocksRepository : IStocksRepository
    {
        private readonly IStocksLoader _stocksLoader;
        private readonly ICachedRepository _redisCache;

        public StocksRepository(IStocksLoader stocksLoader, ICachedRepository redisCache)
        {
            _stocksLoader = stocksLoader;
            _redisCache = redisCache;
        }

        public async Task<IEnumerable<StockDividend>> GetStocks()
        {
            var key = $"{DateTime.Now:yyyy-MM-dd}_{nameof(GetStocks)}";
            var stockDividends = _redisCache.ReadFromCache<IEnumerable<StockDividend>>(key);
            if (stockDividends is null)
            {
                var stocks = await _redisCache.ReadFromCacheAsync<string[]>("StocksOfInterest");
                stockDividends = await _stocksLoader.GetStockDividendsAsync(stocks);
                await _redisCache.WriteToCacheAsync(key, stockDividends);
            }
            return stockDividends;
        }
    }
}
