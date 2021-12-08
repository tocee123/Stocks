using Stocks.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stocks.Core
{
    public class StocksRepository : IStocksRepository
    {
        private readonly IStocksLoader _stocksLoader;
        private readonly IStocksOfInterestRespository _stocksOfInterestRespository;

        public StocksRepository(IStocksLoader stocksLoader, IStocksOfInterestRespository stocksOfInterestRespository)
        {
            _stocksLoader = stocksLoader;
            _stocksOfInterestRespository = stocksOfInterestRespository;
        }

        public async Task<IEnumerable<StockDividend>> GetStocksAsync()
        {
            var tickers = _stocksOfInterestRespository.GetTickers();
            var stockDividends = await _stocksLoader.GetStockDividendsAsync(tickers);
            return stockDividends;
        }
    }
}
