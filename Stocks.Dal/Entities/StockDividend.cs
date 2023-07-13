namespace Stocks.Dal.Entities;

public class StockDividend
{
    public int Id { get; set; }
    public int StockId { get; set; }
    public Stock Stock { get; set; }
    public DateTime ExDividend { get; set; }
    public DateTime PayoutDate { get; set; }
    public double Amount { get; set; }
    public string  Note { get; set; }
}