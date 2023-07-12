namespace Stocks.Dal.Entities;
public class Stock
{
    public int Id { get; set; }
    public string Ticker { get; set; }
    public string Name { get; set; }

    public ICollection<StockDividend> StockDividends { get; private set; } = new List<StockDividend>();
    public ICollection<StockPrice> StockPrices { get; private set; } = new List<StockPrice>();

    public void AddStockDividends(IEnumerable<StockDividend> dividends)
    {
        foreach (var dividend in dividends)
        {
            StockDividends.Add(dividend);
        }
    }

    public void AddPrice(StockPrice price)
    {
        StockPrices.Add(price);
    }
}
