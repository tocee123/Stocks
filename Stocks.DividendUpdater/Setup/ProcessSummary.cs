namespace Stocks.DividendUpdater.Setup;
public class ProcessSummary
{
    public int IsDeletedUpdatedCount { get; set; }
    public int NewDividendPaymentsCount { get; set; }
    public int NewPriceCount { get; set; }

    public override string ToString()
    => $"Summary:\nCount of isDeleted: {IsDeletedUpdatedCount}\nCount of new dividend payments: {NewDividendPaymentsCount}\nCount of new prices: {NewPriceCount}";
}