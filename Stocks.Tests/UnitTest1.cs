using Microsoft.Extensions.Caching.Memory;
using System.IO;
using System.Net;
using System.Net.Http;

namespace WebDownloading.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        //TODO
        //[Test]
        public async Task Test2()
        {

            async Task<bool> DoesPageExist(string url)
            {
                var result = false;
                using var client = new HttpClient();
                var response = await client.GetAsync(url);
                result = response.IsSuccessStatusCode;
                return result;
            }

            //var url = "https://www.etoro.com/markets/edp.lsb";
            //var url = "https://www.etoro.com/search/tef";
            var url = "https://www.etoro.com/search/tef?__cf_chl_captcha_tk__=pmd_9gFfEkSiMUvYiF59A37vZGbfyC0T0rDWZILbhz0N7s0-1635317134-0-gqNtZGzNAyWjcnBszQaR";

            Console.WriteLine(await DoesPageExist(url));
            WebClient client = new WebClient();

            // Add a user agent header in case the 
            // requested URI contains a query.

            using (HttpClient _client = new HttpClient() { Timeout = TimeSpan.FromSeconds(2) })
            {
                _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/html"));
                using (HttpResponseMessage _responseMsg = await _client.GetAsync(url))
                {
                    using (HttpContent content = _responseMsg.Content)
                    {
                        var c = await content.ReadAsStringAsync();
                    }
                }
            }
            var client1 = new HttpClient();

            var content1 = new FormUrlEncodedContent(new[] { KeyValuePair.Create("xxx", "__cf_chl_captcha_tk__=pmd_X7tf4hreIO2u5.GS6wGGkQYbm2VE4S_Vo_NmvsCcgIQ-1635317277-0-gqNtZGzNAyWjcnBszQaR") });

            var response = await client1.PostAsync("https://www.etoro.com/search/tef", content1);

            var responseString = await response.Content.ReadAsStringAsync();

        }

        [Test]
        public async Task InMemoryCacheTest()
        {
            var StocksOfInterest = new[] {"WMB", "MPLX", "EPD", "ET", "AGNC", "NLY", "LUMN", "MFA", "TWO", "GLAD", "PSEC", "LTC", "TSN", "THQ", "KMI", "MO", "IRM", "NYMT", "TEF", "ADT", "AEG", "PBCT", "NWBI", "SGU", "AQN", "HBAN", "ACRE", "ABR", "BPY", "TFSL", "HNNA", "ISBC", "FNCB", "TAIT", "O", "LTC", "STAG", "DX", "MAIN", "PSEC", "DLY", "TEAF", "BTT", "ADC", "LAND", "PBA", "SLG", "DXC", "PG", "KO", "XOM", "ACRE", "ABR", "BPY", "TFSL", "HNNA", "HTBK", "ISBC", "MCBC", "FNCB", "UBCP", "LSBK", "TAIT", "FCF", "FGBI", "CULP", "HPE", "KRNY", "NFBK", "FMBI", "KBAL", "FMNB", "OTTW", "COG", "FFNW", "RVSB", "SHBI", "ASRV", "WVFC", "SBFG", "CBAN", "HFBL", "HCKT", "OVLY", "GWRS", "MWA", "MRTN", "SIRI",
            //champions
            "PBCT", "TDS", "FLIC", "WEYS", "ORI", "T", "BEN", "MDU", "EBTC", "UBSI", "MATW", "SBSI", "AROW", "NJR", "ENB", "ARTN.A", "THFF", "CTBI", "OZK", "HRL", "UGI", "SRCE", "NNN", "LEG", "ABM", "WBA", "UVV", "WTRG", "NWN", "NFG", "CAH", "BRC", "XOM", "AFL", "BANF", "KO", "WABC", "JW.A", "BRO", "UHT", "MCY", "ADM", "SEIC", "CWT", "SON", "DCI", "FUL", "CBSH", "SJW", "MGRC", "BKH", "O", "AOS", "BF.B", "EV", "CBU", "TNC", "ED", "CL", "SYY", "TMP", "MGEE", "RPM", "RTX", "CHD", "FELE", "NEE", "MKC.V", "UMBF", "AWR", "CVX", "ATO", "EMR", "BMI", "RLI", "CFR", "NUE", "SCL", "PII", "TRI", "CNI", "CINF", "FRT", "EXPD", "GPC", "MDT", "ATR", "LECO", "IBM", "KMB", "PG", "WMT", "RNR", "PPG", "PEP", "MSA", "CLX", "JNJ", "DOV", "LANC", "JKHY", "ERIE", "CB", "SWK", "MMM", "GD", "LOW", "ADP", "CAT", "CSL", "TROW", "ECL", "ITW", "MCD", "ALB", "NDSN", "TGT", "BDX", "APD", "SYK", "PH", "SHW", "LIN", "ESS", "CTAS", "GWW", "SPGI", "WST", "ROP" };

            var myCache = new MemoryCache(new MemoryCacheOptions());
            var timespan = TimeSpan.FromSeconds(5);
            string[] result;

            myCache.Set(nameof(StocksOfInterest), StocksOfInterest, timespan);
            do
            {
                result = myCache.Get<string[]>(nameof(StocksOfInterest));
                await Task.Delay(1000);
                Console.WriteLine("waiting");
            } while (result != null);


        }

        //TODO
        [Test, Ignore("something is failing")]
        public async Task Test1()
        {
            var StocksOfInterest = new[] {"WMB", "MPLX", "EPD", "ET", "AGNC", "NLY", "LUMN", "MFA", "TWO", "GLAD", "PSEC", "LTC", "TSN", "THQ", "KMI", "MO", "IRM", "NYMT", "TEF", "ADT", "AEG", "PBCT", "NWBI", "SGU", "AQN", "HBAN", "ACRE", "ABR", "BPY", "TFSL", "HNNA", "ISBC", "FNCB", "TAIT", "O", "LTC", "STAG", "DX", "MAIN", "PSEC", "DLY", "TEAF", "BTT", "ADC", "LAND", "PBA", "SLG", "DXC", "PG", "KO", "XOM", "ACRE", "ABR", "BPY", "TFSL", "HNNA", "HTBK", "ISBC", "MCBC", "FNCB", "UBCP", "LSBK", "TAIT", "FCF", "FGBI", "CULP", "HPE", "KRNY", "NFBK", "FMBI", "KBAL", "FMNB", "OTTW", "COG", "FFNW", "RVSB", "SHBI", "ASRV", "WVFC", "SBFG", "CBAN", "HFBL", "HCKT", "OVLY", "GWRS", "MWA", "MRTN", "SIRI",
            //champions
            "PBCT", "TDS", "FLIC", "WEYS", "ORI", "T", "BEN", "MDU", "EBTC", "UBSI", "MATW", "SBSI", "AROW", "NJR", "ENB", "ARTN.A", "THFF", "CTBI", "OZK", "HRL", "UGI", "SRCE", "NNN", "LEG", "ABM", "WBA", "UVV", "WTRG", "NWN", "NFG", "CAH", "BRC", "XOM", "AFL", "BANF", "KO", "WABC", "JW.A", "BRO", "UHT", "MCY", "ADM", "SEIC", "CWT", "SON", "DCI", "FUL", "CBSH", "SJW", "MGRC", "BKH", "O", "AOS", "BF.B", "EV", "CBU", "TNC", "ED", "CL", "SYY", "TMP", "MGEE", "RPM", "RTX", "CHD", "FELE", "NEE", "MKC.V", "UMBF", "AWR", "CVX", "ATO", "EMR", "BMI", "RLI", "CFR", "NUE", "SCL", "PII", "TRI", "CNI", "CINF", "FRT", "EXPD", "GPC", "MDT", "ATR", "LECO", "IBM", "KMB", "PG", "WMT", "RNR", "PPG", "PEP", "MSA", "CLX", "JNJ", "DOV", "LANC", "JKHY", "ERIE", "CB", "SWK", "MMM", "GD", "LOW", "ADP", "CAT", "CSL", "TROW", "ECL", "ITW", "MCD", "ALB", "NDSN", "TGT", "BDX", "APD", "SYK", "PH", "SHW", "LIN", "ESS", "CTAS", "GWW", "SPGI", "WST", "ROP" };

            var redis = new OfflineCachedRepository();
            await redis.SetAsync(nameof(StocksOfInterest), StocksOfInterest);

            var excelWriter = new StockExcelWriter(new StocksRepository(new StocksLoader(new StockDividendHistoryLoader()), new StocksOfInterestRespository()), new ExcelSaver());
            var bytes = await excelWriter.WriteToExcelAsync();
            await File.WriteAllBytesAsync(@"C:\temp\test1.xlsx", bytes);
        }
    }
}