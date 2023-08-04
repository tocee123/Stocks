using Stocks.Test.HelperClasses;

namespace Stocks.Test.Stocks.Core.Repositories
{
    [TestFixture]
    public class StocksRepositoryShould
    {
        private StocksRepository _target;

        [SetUp]
        public void Setup()
        {
            _target = new StocksRepository(StockContextInMemory.Create().AddStock());
        }

        [Test]
        public async Task ReturnNotEmptyList()
        {
            var result = await _target.GetStocksAsync();
            result.Should().NotBeEmpty();
            result.Count().Should().Be(1);
            result.First().DividendHistories.Should().NotBeEmpty();
            result.First().DividendHistories.Count().Should().Be(15);
        }
    }
}