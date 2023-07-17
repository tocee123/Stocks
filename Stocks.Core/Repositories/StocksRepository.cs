using Microsoft.EntityFrameworkCore;
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
            var result = 
                from stock in _context.Stock
                select new StockDividend
                {
                    Name = stock.Name,
                    Ticker = stock.Ticker,
                    Price = _context.StockPrice.OrderByDescending(sp => sp.Date).First(sp => sp.StockId == stock.Id).Price,
                    IsCorrectlyDownloaded = true,
                    DividendHistories = _context.StockDividend.Where(sd => sd.StockId == stock.Id).OrderByDescending(sd=>sd.ExDividend).Take(15).Select(sd => new DividendHistory { 
                    Amount = sd.Amount,
                    Type = sd.Note,
                    DeclarationDate = sd.ExDividend.AddDays(-1),
                    ExDate = sd.ExDividend,
                    PayDate = sd.PayoutDate,
                    RecordDate = sd.ExDividend.AddDays(1)
                    }).ToArray(),
                };

            return await result.ToArrayAsync();
        }
    }
}
