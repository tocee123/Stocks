using Stocks.Core.DividendDisplay;

namespace Stocks.Test.Core;
public class DividendDisplayTests
{
    [Test]
    public void GenerateMonth_GeneratesMonth()
    {
        var target = new CalendarGenerator();
        var result = target.GenerateMonth();
        result.Should().NotBeNullOrEmpty();
    }
}