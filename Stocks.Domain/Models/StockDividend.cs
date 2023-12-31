﻿using Newtonsoft.Json;
using System.Diagnostics;

namespace Stocks.Domain.Models
{
    [DebuggerDisplay("{Name} {Ticker}")]
    public class StockDividend
    {
        public string Name { get; set; }
        public string Ticker { get; set; }
        public double Price { get; set; }
        public IEnumerable<DividendHistory> DividendHistories { get; set; } = new List<DividendHistory>();
        [JsonIgnore]
        public DividendHistory LatestDividendHistory { get => DividendHistories.OrderByDescending(dh => dh.ExDate).FirstOrDefault() ?? new DividendHistory(); }
        [JsonIgnore]
        public double DividendToPrice { get => LatestDividendHistory.Amount / Price; }
        public bool IsCorrectlyDownloaded { get; set; } = false;
        [JsonIgnore]
        public bool HasSpecial { get => DividendHistories.Any(dh => dh.HasSpecial); }
    }
}
