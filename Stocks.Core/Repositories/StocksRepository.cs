using Stocks.Dal;

namespace Stocks.Core.Repositories
{
    public class StocksRepository : IStocksRepository
    {
        private readonly StockContext _context;

        public StocksRepository(StockContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StockDividend>> GetStocksAsync()
        {
            return new StockDividend[0];
        }
    }
}
