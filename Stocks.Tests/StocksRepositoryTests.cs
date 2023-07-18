using Stocks.Test.HelperClasses;

namespace WebDownloading.Test
{
    [TestFixture]
    public class StocksRepositoryTests
    {
        private StocksRepository _target;
        [SetUp]
        public void Setup()
        {
            _target = new StocksRepository(StockContextInMemory.Create().AddStock());
        }

        [Test]
        public async Task GetStocks_ReturnsNotEmptyList()
        {
            var result = await _target.GetStocksAsync();
            result.Should().NotBeEmpty();
            result.Count().Should().Be(1);
            result.First().DividendHistories.Should().NotBeEmpty();
            result.First().DividendHistories.Count().Should().Be(15);
        }
    }
}