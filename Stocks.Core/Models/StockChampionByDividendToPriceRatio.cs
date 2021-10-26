using System;

namespace Stocks.Core.Models
{
    public record StockChampionByDividendToPriceRatio(string Name, string Ticker, DateTime ExDate, double DividendToPrice, double Price, double Dividend)
    {
        public double InvestedInCurrentMonth { get; set; }
        public int StockVolume { get => (int)(InvestedInCurrentMonth / Price); }
        public double Earnings { get => StockVolume * Dividend; }
    }
}
