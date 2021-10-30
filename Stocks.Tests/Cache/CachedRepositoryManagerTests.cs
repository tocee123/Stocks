using NSubstitute;
using NUnit.Framework;
using Stocks.Core.Cache;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stocks.Test.Cache
{
    [TestFixture]
    public class CachedRepositoryManagerTests
    {
        private ICachedRepositoryManager _target;
        ICachedRepository _cachedRepositorySubstitute;

        [SetUp]
        public void Setup()
        {
            _cachedRepositorySubstitute = Substitute.For<ICachedRepository>();
            _target = new CachedRepositoryManager(_cachedRepositorySubstitute);
        }

        [Test]
        public async Task GetStocksOfInterest_WhenStocksOfInterestIsEmpty_CallMethodToFillIt()
        {
            var key = StocksOfInterest.Key;
            _cachedRepositorySubstitute.ReadFromCacheAsync<IEnumerable<string>>(key).Returns(null, new string[] { "test" });

            var stocksOfInterest = await _target.GetStocksOfInterestAsync();

            Assert.IsNotNull(stocksOfInterest);
            Assert.IsTrue(stocksOfInterest.Any());
            await _cachedRepositorySubstitute.Received().WriteToCacheAsync(key, Arg.Any<string>());
        }
    }
}
