using System;

namespace Stocks.Core.Models
{
    public record StockChampionByDividendToPriceRatio(string Name, string ShortName, DateTime ExDate, double DividendToPrice, double Price, double Dividend);
}
