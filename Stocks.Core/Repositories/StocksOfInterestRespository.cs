using Stocks.Core.Cache;
using System.Collections.Generic;

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
