using NUnit.Framework;
using Stocks.Core.Cache;
using System.Linq;

namespace Stocks.Test.Cache
{
    [TestFixture]
    public class CachedRepositoryManagerTests
    {
        private CachedRepositoryManager _target;

        [SetUp]
        public void Setup()
        {
            _target = new CachedRepositoryManager();
        }
        
        [Test]
        public void GetStocksOfInterest_WhenStocksOfInterestIsEmpty_CallMethodToFillIt()
        {
            var stocksOfInterest = _target.GetStocksOfInterest();
            Assert.IsNotNull(stocksOfInterest);
            Assert.IsTrue(stocksOfInterest.Any());
        }
    }
}
