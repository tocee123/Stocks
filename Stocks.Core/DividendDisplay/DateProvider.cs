namespace Stocks.Core.DividendDisplay;

public class DateProvider : IDateProvider
{
    public DateTime GetToday()
        => DateTime.Today;
}
