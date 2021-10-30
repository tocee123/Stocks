﻿using Newtonsoft.Json;
using Stocks.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stocks.Core.Cache
{
    public class CachedRepositoryManager : ICachedRepositoryManager
    {
        private ICachedRepository _cachedRepository;

        public CachedRepositoryManager(ICachedRepository cachedRepository)
        {
            _cachedRepository = cachedRepository;
        }

        public async Task<IEnumerable<string>> GetStocksOfInterestAsync()
        {
            var key = StocksOfInterest.Key;
            async Task<string[]> getStocksOfInterest() => await _cachedRepository.ReadFromCacheAsync<string[]>(key);

            var stocksOfInterest = await getStocksOfInterest();
            if (stocksOfInterest?.Any() ?? false == false)
            {
                await _cachedRepository.WriteToCacheAsync(key, StocksOfInterest.Stocks);
            }
            return await getStocksOfInterest();
        }

        public async Task<IEnumerable<StockDividend>> GetStockDividendsAsync()
        => await _cachedRepository.ReadFromCacheAsync<IEnumerable<StockDividend>>(CreateKey());

        private string CreateKey() => $"{DateTime.Now:yyyy-MM-dd}_GetStocks";
    }
}
