using Stocks.Core.DividendDisplay;

namespace Stocks.Test.Core.DividendDisplay;
public class CalendarGeneratorShould
{
    private ICalendarGenerator _target;
    private IStocksRepository _stocksRepository;
    private IDateProvider _dateProvider;

    [SetUp]
    public void Setup()
    {
        _stocksRepository = Substitute.For<IStocksRepository>();
        _dateProvider = Substitute.For<IDateProvider>();
        _target = new CalendarGenerator(_dateProvider, _stocksRepository);
    }

    [Test]
    public async Task GenerateMonth()
    {
        var today = DateTime.Today;
        var fixture = new Fixture().SetupFixtureToGenerateDateInCurrentMonth(today.Year, today.Month);
        _stocksRepository.GetStocksAsync().Returns(fixture.CreateMany<StockDividend>());

        _dateProvider.GetToday().Returns(today);

        var result = await _target.GenerateMonthAsync();
        result.Should().NotBeNullOrEmpty();
        result.Sum(x => x.Count()).Should().BeGreaterThanOrEqualTo(1);
    }
}
