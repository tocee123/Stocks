using NUnit.Framework;
using Stocks.Core;
using Stocks.Core.Excel;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebDownloading.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
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

            var url = "https://www.etoro.com/search/tef";

            await DoesPageExist(url);

        }

        [Test]
        public async Task Test1()
        {
            var StocksOfInterest = new[] {"WMB", "MPLX", "EPD", "ET", "AGNC", "NLY", "LUMN", "MFA", "TWO", "GLAD", "PSEC", "LTC", "TSN", "THQ", "KMI", "MO", "IRM", "NYMT", "TEF", "ADT", "AEG", "PBCT", "NWBI", "SGU", "AQN", "HBAN", "ACRE", "ABR", "BPY", "TFSL", "HNNA", "ISBC", "FNCB", "TAIT", "O", "LTC", "STAG", "DX", "MAIN", "PSEC", "DLY", "TEAF", "BTT", "ADC", "LAND", "PBA", "SLG", "DXC", "PG", "KO", "XOM", "ACRE", "ABR", "BPY", "TFSL", "HNNA", "HTBK", "ISBC", "MCBC", "FNCB", "UBCP", "LSBK", "TAIT", "FCF", "FGBI", "CULP", "HPE", "KRNY", "NFBK", "FMBI", "KBAL", "FMNB", "OTTW", "COG", "FFNW", "RVSB", "SHBI", "ASRV", "WVFC", "SBFG", "CBAN", "HFBL", "HCKT", "OVLY", "GWRS", "MWA", "MRTN", "SIRI",
            //champions
            "PBCT", "TDS", "FLIC", "WEYS", "ORI", "T", "BEN", "MDU", "EBTC", "UBSI", "MATW", "SBSI", "AROW", "NJR", "ENB", "ARTN.A", "THFF", "CTBI", "OZK", "HRL", "UGI", "SRCE", "NNN", "LEG", "ABM", "WBA", "UVV", "WTRG", "NWN", "NFG", "CAH", "BRC", "XOM", "AFL", "BANF", "KO", "WABC", "JW.A", "BRO", "UHT", "MCY", "ADM", "SEIC", "CWT", "SON", "DCI", "FUL", "CBSH", "SJW", "MGRC", "BKH", "O", "AOS", "BF.B", "EV", "CBU", "TNC", "ED", "CL", "SYY", "TMP", "MGEE", "RPM", "RTX", "CHD", "FELE", "NEE", "MKC.V", "UMBF", "AWR", "CVX", "ATO", "EMR", "BMI", "RLI", "CFR", "NUE", "SCL", "PII", "TRI", "CNI", "CINF", "FRT", "EXPD", "GPC", "MDT", "ATR", "LECO", "IBM", "KMB", "PG", "WMT", "RNR", "PPG", "PEP", "MSA", "CLX", "JNJ", "DOV", "LANC", "JKHY", "ERIE", "CB", "SWK", "MMM", "GD", "LOW", "ADP", "CAT", "CSL", "TROW", "ECL", "ITW", "MCD", "ALB", "NDSN", "TGT", "BDX", "APD", "SYK", "PH", "SHW", "LIN", "ESS", "CTAS", "GWW", "SPGI", "WST", "ROP" };

            var redis = new RedisOfflineCache();
            await redis.WriteToCacheAsync(nameof(StocksOfInterest), StocksOfInterest);

            var excelWriter = new StockExcelWriter(new StocksRepository(new StocksLoader(new StockDividendHistoryLoader()), new RedisOfflineCache()), new ExcelSaver());
            var bytes = await excelWriter.WriteToExcelAsync();
            await File.WriteAllBytesAsync(@"C:\temp\test1.xlsx", bytes);
        }
    }
}