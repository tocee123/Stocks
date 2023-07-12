namespace Stocks.Dal.Entities;
public class Stock
{
    public int Id { get; set; }
    public string Ticker { get; set; }
    public string Name { get; set; }

    public ICollection<StockDividend> StockDividends { get; private set; } = new List<StockDividend>();
}
