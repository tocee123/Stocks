using Stocks.Core.DividendDisplay;

namespace Stocks.Test.Core;
public class DividendDisplayTests
{
    [Test]
    public void GenerateMonth_GeneratesMonth()
    {
        var today = DateTime.Today;

        var dateProvider = Substitute.For<IDateProvider>();
        var target = new CalendarGenerator(dateProvider);

        dateProvider.GetToday().Returns(today);

        var result = target.GenerateMonth();
        result.Should().NotBeNullOrEmpty();
        result.Sum(x=>x.Count()).Should().BeGreaterThanOrEqualTo(DateTime.DaysInMonth(today.Year, today.Month));
    }
}