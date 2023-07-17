using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Stocks.Dal;

public class StockContextFactory : IDesignTimeDbContextFactory<StockContext>
{
    public StockContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json")
           .AddUserSecrets(Assembly.GetExecutingAssembly())
        .Build();

        var optionsBuilder = new DbContextOptionsBuilder<StockContext>();
        var connectionString = configuration.GetConnectionString("StockWebDividendDB");
        optionsBuilder.UseSqlServer(connectionString);

        return new StockContext(optionsBuilder.Options);
    }
}