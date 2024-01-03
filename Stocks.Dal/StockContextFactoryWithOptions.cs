using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Stocks.Dal;
public class StockContextFactoryWithOptions : IDbContextFactory<StockContext>
{
    private readonly string _connectionString;

    public StockContextFactoryWithOptions(IOptionsMonitor<DbAccess> connectionString)
    {
        _connectionString = connectionString.Get(DbAccess.StockWebDividendDB).ConnectionString;
    }

    public StockContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<StockContext>();
        optionsBuilder.UseSqlServer(_connectionString);

        return new StockContext(optionsBuilder.Options);
    }
}

