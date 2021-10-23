using NUnit.Framework;
using Stocks.Core;
using Stocks.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebDownloading.Test
{
    [TestFixture]
    public class DividendByMonthCollectionTest

    {
        private StocksRepository _target;
        [SetUp]
        public void Setup()
        {
            _target = new StocksRepository(new StocksLoader(new StockDividendHistoryLoader()), new RedisCache());
        }

        [Test]
        public async Task SelectTop1DividnedByMonth()
        {
            var year = 2021;

            var stocks = (await _target.GetStocks()).SelectMany(s => s.DividendHistories, (s, dh) => new { s.Name, dh.ExDate, DividendToPrice = Math.Round(dh.Amount / s.Price, 4), s.Price, dh.Amount, s.ShortName }).Where(s => s.ExDate.Year == year);

            var grouppedByMonth = stocks.GroupBy(s => new { s.ExDate.Month, s.ExDate.Year }).Select(s => new { s.Key, TopDividendPayer = s.OrderByDescending(so => so.DividendToPrice).FirstOrDefault() });
            Console.WriteLine("Monthly best ratios:");

            var investment = 1000;
            Console.WriteLine($"if you would invest {investment}$, you would get:");
            int calculateStockCountFromPrice(double price) => (int)(investment / price);
            var sum = 0.0;
            foreach (var item in grouppedByMonth)
            {
                var amountOfDividend = Math.Round(calculateStockCountFromPrice(item.TopDividendPayer.Price) * item.TopDividendPayer.Amount, 2);
                sum += amountOfDividend;
                Console.WriteLine($"In {item.Key.Year}/{item.Key.Month}: {item.TopDividendPayer.Name} '{item.TopDividendPayer.ShortName}' with {item.TopDividendPayer.Price}$, dividend {item.TopDividendPayer.Amount}$ ratio: {item.TopDividendPayer.DividendToPrice:0.00%} you could buy {calculateStockCountFromPrice(item.TopDividendPayer.Price)} stocks and get {amountOfDividend}$ on { item.TopDividendPayer.ExDate.ToShortDateString()}");
            }
            Console.WriteLine($"{sum}$");
        }
    }
}