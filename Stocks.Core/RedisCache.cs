﻿using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Stocks.Core
{
    public class RedisCache : IRedisCache
    {
        const string _redisConnectionString = "realking.redis.cache.windows.net:6380,password=01aiSPXiOXFKmdYyIxFdpxOZcyDYbzgDFcFmhWau6Xc=,ssl=True,abortConnect=False";
        public T ReadFromCache<T>(string key)
        {
            using var cm = ConnectionMultiplexer.Connect(_redisConnectionString);
            var db = cm.GetDatabase();
            string value = db.StringGet(key);
            return value is null ? default(T)
                : JsonConvert.DeserializeObject<T>(value);
        }

        public async Task<T> ReadFromCacheAsync<T>(string key)
        {
            var value = await ReadStringFromCacheAsync(key);
            return value is null ? default(T)
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
            var StocksOfInterest = new[] { "WMB", "MPLX", "EPD", "ET", "AGNC", "NLY", "LUMN", "MFA", "TWO", "GLAD", "PSEC", "LTC", "TSN", "THQ", "KMI", "MO", "IRM", "NYMT", "TEF", "ADT", "AEG", "PBCT", "NWBI", "SGU", "AQN", "HBAN", "ACRE", "ABR", "BPY", "TFSL", "HNNA", "ISBC", "FNCB", "TAIT", "O", "LTC", "STAG", "DX", "MAIN", "PSEC", "DLY", "TEAF", "BTT", "ADC", "LAND", "PBA", "SLG", "DXC", "PG", "KO", "XOM", "ACRE", "ABR", "BPY", "TFSL", "HNNA", "HTBK", "ISBC", "MCBC", "FNCB", "UBCP", "LSBK", "TAIT", "FCF", "FGBI", "CULP", "HPE", "KRNY", "NFBK", "FMBI", "KBAL", "FMNB", "OTTW", "COG", "FFNW", "RVSB", "SHBI", "ASRV", "WVFC", "SBFG", "CBAN", "HFBL", "HCKT", "OVLY", "GWRS", "MWA", "MRTN", "SIRI",
            //champions
            "PBCT", "TDS", "FLIC", "WEYS", "ORI", "T", "BEN", "MDU", "EBTC", "UBSI", "MATW", "SBSI", "AROW", "NJR", "ENB", "ARTN.A", "THFF", "CTBI", "OZK", "HRL", "UGI", "SRCE", "NNN", "LEG", "ABM", "WBA", "UVV", "WTRG", "NWN", "NFG", "CAH", "BRC", "XOM", "AFL", "BANF", "KO", "WABC", "JW.A", "BRO", "UHT", "MCY", "ADM", "SEIC", "CWT", "SON", "DCI", "FUL", "CBSH", "SJW", "MGRC", "BKH", "O", "AOS", "BF.B", "EV", "CBU", "TNC", "ED", "CL", "SYY", "TMP", "MGEE", "RPM", "RTX", "CHD", "FELE", "NEE", "MKC.V", "UMBF", "AWR", "CVX", "ATO", "EMR", "BMI", "RLI", "CFR", "NUE", "SCL", "PII", "TRI", "CNI", "CINF", "FRT", "EXPD", "GPC", "MDT", "ATR", "LECO", "IBM", "KMB", "PG", "WMT", "RNR", "PPG", "PEP", "MSA", "CLX", "JNJ", "DOV", "LANC", "JKHY", "ERIE", "CB", "SWK", "MMM", "GD", "LOW", "ADP", "CAT", "CSL", "TROW", "ECL", "ITW", "MCD", "ALB", "NDSN", "TGT", "BDX", "APD", "SYK", "PH", "SHW", "LIN", "ESS", "CTAS", "GWW", "SPGI", "WST", "ROP"
            };

            var convertedValue = JsonConvert.SerializeObject(StocksOfInterest);
            using var cm = ConnectionMultiplexer.Connect(_redisConnectionString);
            var db = cm.GetDatabase();
            await db.StringSetAsync(nameof(StocksOfInterest), convertedValue);
        }
    }
}
