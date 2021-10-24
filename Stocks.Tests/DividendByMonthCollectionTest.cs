﻿using NUnit.Framework;
using Stocks.Core;
using Stocks.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            var stocks = (await _target.GetStocks()).SelectMany(s => s.DividendHistories, (s, dh) => new { s.Name, dh.ExDate, DividendToPrice = Math.Round(dh.Amount / s.Price, 4), s.Price, dh.Amount, s.ShortName });

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