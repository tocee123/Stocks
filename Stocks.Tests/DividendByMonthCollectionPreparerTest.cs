using NUnit.Framework;
using Stocks.Core;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDownloading.Test
{
    [TestFixture]
    public class DividendByMonthCollectionPreparerTest

    {
        private IDividendByMonthCollectionPreparer _target;

        private StocksRepository _stockRepository;
        [SetUp]
        public void Setup()
        {
            _stockRepository = new StocksRepository(new StocksLoader(new StockDividendHistoryLoader()), new RedisCache());
            _target = new DividendByMonthCollectionPreparer(_stockRepository);
        }

        [Test]
        public async Task GetMonthlyBestStocksByYear_ReturnsNotNullList()
        {
            var result = await _target.GetMonthlyBestStocksByYear(DateTime.Today.Year);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(12, result.Count());
        }

        [Test]
        public async Task SelectTop1DividnedByMonth()
        {
            var stocks = (await _stockRepository.GetStocks()).SelectMany(s => s.DividendHistories, (s, dh) => new { s.Name, dh.ExDate, DividendToPrice = Math.Round(dh.Amount / s.Price, 4), s.Price, dh.Amount, s.ShortName });

            var grouppedByMonth = stocks.GroupBy(s => new { s.ExDate.Month, s.ExDate.Year }).Select(s => new { s.Key, TopDividendPayer = s.OrderByDescending(so => so.DividendToPrice).FirstOrDefault() });
            Console.WriteLine("Monthly best ratios:");

            var investment = 1000;
            Console.WriteLine($"if you would invest {investment}$, you would get:");
            int calculateStockCountFromPrice(double price) => (int)(investment / price);
            var sum = 0.0;
            var sb = new StringBuilder();
            foreach (var item in grouppedByMonth.OrderByDescending(x => x.Key.Year).ThenByDescending(x => x.Key.Month))
            {
                var amountOfDividend = Math.Round(calculateStockCountFromPrice(item.TopDividendPayer.Price) * item.TopDividendPayer.Amount, 2);
                sum += amountOfDividend;
                sb.AppendLine($"In {item.Key.Year}/{item.Key.Month}: {item.TopDividendPayer.Name} '{item.TopDividendPayer.ShortName}' with {item.TopDividendPayer.Price}$, dividend {item.TopDividendPayer.Amount}$ ratio: {item.TopDividendPayer.DividendToPrice:0.00%} you could buy {calculateStockCountFromPrice(item.TopDividendPayer.Price)} stocks and get {amountOfDividend}$ on { item.TopDividendPayer.ExDate.ToShortDateString()}");
            }
            sb.AppendLine($"{sum}$");
            File.WriteAllText(@"c:\temp\test111.txt", sb.ToString());
        }
    }
}