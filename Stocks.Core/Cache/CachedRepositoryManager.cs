﻿using Newtonsoft.Json;
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
            async Task<IEnumerable<string>> getStocksOfInterest() => await _cachedRepository.ReadFromCacheAsync<IEnumerable<string>>(key);

            var stocksOfInterest = await getStocksOfInterest();
            if (stocksOfInterest?.Any() ?? false == false)
            {
                var stockOfInterestJson = JsonConvert.SerializeObject(StocksOfInterest.Stocks);
                await _cachedRepository.WriteToCacheAsync(key, stockOfInterestJson);
            }
            return await getStocksOfInterest();
        }
    }
}
