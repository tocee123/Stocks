using Stocks.Core.Cache;

namespace Stocks.Core.Repositories
{
    public class StocksOfInterestRespository : IStocksOfInterestRespository
    {
        public IEnumerable<string> GetTickers()
        {
            return StocksOfInterest.Stocks;
        }
    }
}
