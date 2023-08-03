using Stocks.Core.Enums;
using Stocks.Test;

namespace WebDownloading.Test
{
    [TestFixture]
    public class RedisCacheTests
    {
        private RedisCachedRepository _target;
        [SetUp]
        public void Setup()
        {
            _target = new RedisCachedRepository(ConfigurationBuilderBuilder.GetOptions());
        }

        [Test]
        public async Task WriteToCache_WritesObjectInJsonFormat()
        {
            var fixture = new Fixture();
            var stockDividends = fixture.Create<IEnumerable<StockDividend>>();
            var key = $"{DateTime.Now.ToString("yyyy-MM-dd")}_{nameof(WriteToCache_WritesObjectInJsonFormat)}";
            await _target.SetAsync(key, stockDividends, CacheDuration.OneMinute);
            var result = await _target.GetAsync<IEnumerable<StockDividend>>(key);

            Assert.IsNotNull(result);
        }

        [Test, Ignore("Needs a way to delete the cache")]
        public async Task ReadStringFromCache_WhenKeyExists_ReturnsString()
        {
            var key = "123";
            var result = await _target.GetStringAsync(key);
            Assert.IsNotNull(result);
        }
    }
}