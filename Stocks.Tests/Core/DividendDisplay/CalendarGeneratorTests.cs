using Stocks.Core.DividendDisplay;

namespace Stocks.Test.Core.DividendDisplay;
public class CalendarGeneratorTests
{


    [Test]
    public async Task GenerateMonth_GeneratesMonth()
    {
        var today = DateTime.Today;
        var stockRepository = Substitute.For<IStocksRepository>();
        var dateProvider = Substitute.For<IDateProvider>();
        var target = new CalendarGenerator(dateProvider, stockRepository);

        var fixture = new Fixture().SetupFixtureToGenerateDateInCurrentMonth(today.Year, today.Month);
        stockRepository.GetStocksAsync().Returns(fixture.CreateMany<StockDividend>());

        dateProvider.GetToday().Returns(today);

        var result = await target.GenerateMonthAsync();
        result.Should().NotBeNullOrEmpty();
        result.Sum(x => x.Count()).Should().BeGreaterThanOrEqualTo(1);
    }
}