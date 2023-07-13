using Microsoft.EntityFrameworkCore;

namespace Stocks.Dal.Entities;

public class StockContext : DbContext
{
    public StockContext(DbContextOptions<StockContext> options)
        : base(options)
    {
    }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //=> optionsBuilder.LogTo(Console.WriteLine);

    public DbSet<Stock> Stock { get; set; }
    public DbSet<StockDividend> StockDividend { get; set; }
    public DbSet<StockPrice> StockPrice { get; set; }
    public DbSet<StockOfInterest> StockOfInterest { get; set; }
}
