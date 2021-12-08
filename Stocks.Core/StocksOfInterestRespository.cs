using Stocks.Core.Cache;
using System.Collections.Generic;

namespace Stocks.Core
{
    public class StocksOfInterestRespository : IStocksOfInterestRespository
    {
        public IEnumerable<string> GetTickers()
        {
            return StocksOfInterest.Stocks;
        }
    }
}
