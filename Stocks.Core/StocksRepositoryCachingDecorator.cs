using Stocks.Core.Cache;
using Stocks.Core.Enums;
using Stocks.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stocks.Core
{
    public class StocksRepositoryCachingDecorator : IStocksRepository
    {
        private readonly IStocksRepository _stocksRepository;
        private readonly ICachedRepository _cachedRepository;

        public StocksRepositoryCachingDecorator(IStocksRepository stocksRepository, ICachedRepository cachedRepository)
        {
            _stocksRepository = stocksRepository;
            _cachedRepository = cachedRepository;
        }

        public async Task<IEnumerable<StockDividend>> GetStocksAsync()
        {
            var key = GenerateKey();
            if (await _cachedRepository.ReadFromCacheAsync<IEnumerable<StockDividend>>(key) is var stocks && stocks is null)
            {
                stocks = await _stocksRepository.GetStocksAsync();
                await _cachedRepository.WriteToCacheAsync(key, stocks, CacheDuration.OneHour);
            }
            return stocks;
        }

        private static string GenerateKey() => $"{DateTime.Today.ToString("yyyy-MM-dd")}_{nameof(GetStocksAsync)}";
    }
}
