using System.Collections.Generic;

namespace Stocks.Core
{
    public interface IStocksOfInterestRespository
    {
        IEnumerable<string> GetTickers();
    }
}