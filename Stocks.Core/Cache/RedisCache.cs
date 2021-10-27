﻿using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Stocks.Core.Cache
{
    public class RedisCache : IRedisCache
    {
        const string _redisConnectionString = "stockredis.redis.cache.windows.net:6380,password=jxQt8QqLPu7ezjjBjANAhmVx2v3hftB5YhFwZXqhOHk=,ssl=True,abortConnect=False";
        public T ReadFromCache<T>(string key)
        {
            using var cm = ConnectionMultiplexer.Connect(_redisConnectionString);
            var db = cm.GetDatabase();
            string value = db.StringGet(key);
            return value is null ? default
                : JsonConvert.DeserializeObject<T>(value);
        }

        public async Task<T> ReadFromCacheAsync<T>(string key)
        {
            var value = await ReadStringFromCacheAsync(key);
            return value is null ? default
                : JsonConvert.DeserializeObject<T>(value);
        }

        public async Task<string> ReadStringFromCacheAsync(string key)
        {
            using var cm = ConnectionMultiplexer.Connect(_redisConnectionString);
            var db = cm.GetDatabase();
            return await db.StringGetAsync(key);
        }

        public async Task WriteToCacheAsync<T>(string key, T value, int? expiration = null)
        {
            var expiry = TimeSpan.FromSeconds(expiration ?? 3600);
            var convertedValue = JsonConvert.SerializeObject(value);
            using var cm = ConnectionMultiplexer.Connect(_redisConnectionString);
            var db = cm.GetDatabase();
            await db.StringSetAsync(key, convertedValue, expiry);
        }

        public async Task WriteStocksOfInterestAsync()
        {
            var StocksOfInterest = 0;
            var convertedValue = JsonConvert.SerializeObject(StocksOfInterestEtoro);
            using var cm = ConnectionMultiplexer.Connect(_redisConnectionString);
            var db = cm.GetDatabase();
            await db.StringSetAsync(nameof(StocksOfInterest), convertedValue);
        }

        private static string[] StocksOfInterestEtoro = new[] { "WMB", "MPLX", "EPD", "ET", "AGNC", "NLY", "LUMN", "MFA", "TWO", "PSEC", "TSN", "KMI", "MO", "IRM", "NYMT", "TEF", "ADT", "AEG", "PBCT", "HBAN", "ABR", "ISBC", "O", "LTC", "STAG", "MAIN", "PSEC", "ADC", "PBA", "SLG", "DXC", "PG", "KO", "XOM", "ABR", "ISBC", "HPE", "FFNW", "RVSB", "SHBI", "ASRV", "WVFC", "SBFG", "CBAN", "MWA", "SIRI",
            //champions
            "PBCT", "TDS", "ORI", "T", "BEN", "MDU", "ENB", "HRL", "NNN", "LEG", "ABM", "WBA", "UVV", "CAH", "XOM", "AFL", "KO", "BRO", "ADM", "SEIC", "SON", "DCI", "O", "AOS", "ED", "CL", "SYY", "RPM", "RTX", "CHD","NEE", "CVX", "ATO", "EMR", "BMI", "NUE", "PII", "CNI", "CINF", "FRT", "EXPD", "GPC", "MDT",  "LECO", "IBM", "KMB", "PG", "WMT", "RNR", "PPG", "PEP", "CLX", "JNJ", "DOV", "JKHY", "CB", "SWK", "MMM", "GD", "LOW", "ADP", "CAT", "TROW", "ECL", "ITW", "MCD", "ALB", "TGT", "BDX", "APD", "SYK", "PH", "SHW", "LIN", "ESS", "CTAS", "GWW", "SPGI", "WST", "ROP"
            };

        private static string[] StocksOfItnerestForTony = new[] { "T", "BAX", "KMB", "K", "IBM", "DLR", "KO", "JNJ", "MO", "PG", "WMT", "PEP", "HAS", "PM", "MMM", "BA", "WDC", "CVX", " MCD", "XOM", "RNO.PA", "CBRL", "DIS", "BLK", "TROW", "AXP", "AAPL", "COP", "EBAY", "TGT", "SPG", "JCI", "OXI", "GPS", "APA" };

        private static string[] StocksOfInterestWithChampions = new[] { "WMB", "MPLX", "EPD", "ET", "AGNC", "NLY", "LUMN", "MFA", "TWO", "GLAD", "PSEC", "LTC", "TSN", "THQ", "KMI", "MO", "IRM", "NYMT", "TEF", "ADT", "AEG", "PBCT", "NWBI", "SGU", "AQN", "HBAN", "ACRE", "ABR", "BPY", "TFSL", "HNNA", "ISBC", "FNCB", "TAIT", "O", "LTC", "STAG", "DX", "MAIN", "PSEC", "DLY", "TEAF", "BTT", "ADC", "LAND", "PBA", "SLG", "DXC", "PG", "KO", "XOM", "ACRE", "ABR", "BPY", "TFSL", "HNNA", "HTBK", "ISBC", "MCBC", "FNCB", "UBCP", "LSBK", "TAIT", "FCF", "FGBI", "CULP", "HPE", "KRNY", "NFBK", "FMBI", "KBAL", "FMNB", "OTTW", "COG", "FFNW", "RVSB", "SHBI", "ASRV", "WVFC", "SBFG", "CBAN", "HFBL", "HCKT", "OVLY", "GWRS", "MWA", "MRTN", "SIRI",
            //champions
            "PBCT", "TDS", "FLIC", "WEYS", "ORI", "T", "BEN", "MDU", "EBTC", "UBSI", "MATW", "SBSI", "AROW", "NJR", "ENB", "ARTN.A", "THFF", "CTBI", "OZK", "HRL", "UGI", "SRCE", "NNN", "LEG", "ABM", "WBA", "UVV", "WTRG", "NWN", "NFG", "CAH", "BRC", "XOM", "AFL", "BANF", "KO", "WABC", "JW.A", "BRO", "UHT", "MCY", "ADM", "SEIC", "CWT", "SON", "DCI", "FUL", "CBSH", "SJW", "MGRC", "BKH", "O", "AOS", "BF.B", "EV", "CBU", "TNC", "ED", "CL", "SYY", "TMP", "MGEE", "RPM", "RTX", "CHD", "FELE", "NEE", "MKC.V", "UMBF", "AWR", "CVX", "ATO", "EMR", "BMI", "RLI", "CFR", "NUE", "SCL", "PII", "TRI", "CNI", "CINF", "FRT", "EXPD", "GPC", "MDT", "ATR", "LECO", "IBM", "KMB", "PG", "WMT", "RNR", "PPG", "PEP", "MSA", "CLX", "JNJ", "DOV", "LANC", "JKHY", "ERIE", "CB", "SWK", "MMM", "GD", "LOW", "ADP", "CAT", "CSL", "TROW", "ECL", "ITW", "MCD", "ALB", "NDSN", "TGT", "BDX", "APD", "SYK", "PH", "SHW", "LIN", "ESS", "CTAS", "GWW", "SPGI", "WST", "ROP"
            };

    }
}