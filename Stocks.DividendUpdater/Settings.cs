namespace Stocks.DividendUpdater;
internal class Settings
{
    public const string SectionName = "ConnectionStrings";
    public string StockWebDividendDB { get; set; }
    public string Redis { get; set; }
}