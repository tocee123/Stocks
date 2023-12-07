namespace Stocks.Dal.Entities;

public class StockDividend : IEquatable<StockDividend>
{
    public int Id { get; set; }
    public int StockId { get; set; }
    public Stock Stock { get; set; }
    public DateTime ExDividend { get; set; }
    public DateTime PayoutDate { get; set; }
    public double Amount { get; set; }
    public string Note { get; set; }
    public bool IsDeleted { get; set; }

    public bool Equals(StockDividend other)
    {
        if (other is null)
            return false;

        return ExDividend == other.ExDividend
            && PayoutDate == other.PayoutDate;
    }

    public override bool Equals(object obj) => Equals(obj as StockDividend);
    public override int GetHashCode() => (ExDividend, PayoutDate, Amount).GetHashCode();
}