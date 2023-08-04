using Stocks.Core.Enums;
using Stocks.Test.HelperClasses;

namespace Stocks.Test.Stocks.Core.Cache
{
    [TestFixture]
    public class RedisCachedRepositoryShould
    {
        private RedisCachedRepository _target;
        [SetUp]
        public void Setup()
        {
            _target = new RedisCachedRepository(ConfigurationBuilderBuilder.GetOptions());
        }

        [Test]
        public async Task WriteObjectInJsonFormat()
        {
            var fixture = new Fixture();
            var stockDividends = fixture.Create<IEnumerable<StockDividend>>();
            var key = $"{DateTime.Now.ToString("yyyy-MM-dd")}_{nameof(WriteObjectInJsonFormat)}";
            await _target.SetAsync(key, stockDividends, CacheDuration.OneMinute);
            var result = await _target.GetAsync<IEnumerable<StockDividend>>(key);

            Assert.IsNotNull(result);
        }

        [Test, Ignore("Needs a way to delete the cache")]
        public async Task ReturnValueWhenKeyExists()
        {
            var key = "123";
            var result = await _target.GetStringAsync(key);
            Assert.IsNotNull(result);
        }
    }
}