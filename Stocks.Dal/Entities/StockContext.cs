using Microsoft.EntityFrameworkCore;

namespace Stocks.Dal.Entities;

public class StockContext : DbContext
{
    public StockContext(DbContextOptions<StockContext> options)
        : base(options)
    {
    }

    public DbSet<Stock> Person { get; set; }
}