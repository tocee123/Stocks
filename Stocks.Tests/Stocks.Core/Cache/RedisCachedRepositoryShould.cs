using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Stocks.Core;
using Stocks.Core.Enums;

namespace Stocks.Test.Stocks.Core.Cache
{
    [TestFixture]
    public class RedisCachedRepositoryShould
    {
        private RedisCachedRepository _target;
        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile($"appsettings.Development.json", optional: false)
               .Build();
            var mockedOptions = Substitute.For<IOptionsMonitor<DbAccess>>();
            mockedOptions.Get(DbAccess.Redis).Returns(new DbAccess { ConnectionString = configuration.GetConnectionString("Redis") });
            _target = new RedisCachedRepository(mockedOptions);
        }

        [Test]
        public async Task WriteObjectInJsonFormat()
        {
            var fixture = new Fixture();
            var stockDividends = fixture.Create<IEnumerable<StockDividend>>();
            var key = $"{DateTime.Now.ToString("yyyy-MM-dd")}_{nameof(WriteObjectInJsonFormat)}";
            await _target.SetAsync(key, stockDividends, CacheDuration.OneMinute);
            var result = await _target.GetAsync<IEnumerable<StockDividend>>(key);

            result.Should().NotBeNull();
        }

        [Test, Ignore("Needs a way to delete the cache")]
        public async Task ReturnValueWhenKeyExists()
        {
            var key = "123";
            var result = await _target.GetStringAsync(key);
            result.Should().NotBeNull();
        }
    }
}