using Microsoft.Extensions.DependencyInjection;
using Stocks.Dal;
using Stocks.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using StockDividend = Stocks.Dal.Entities.StockDividend;

namespace Stocks.Test.HelperClasses
{
    public static class StockContextInMemory
    {
        public static string Ticker = "ABC";

        public static StockContext Create()
        {
            var Services = new ServiceCollection();

            Services.AddDbContext<StockContext>(opt => opt.UseInMemoryDatabase($"InMemoryDb_{Guid.NewGuid()}"),
                ServiceLifetime.Scoped,
                ServiceLifetime.Scoped);

            var ServiceProvider = Services.BuildServiceProvider();

            var context = ServiceProvider.GetRequiredService<StockContext>();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            return context;
        }

        public static StockContext AddTicker(this StockContext context)
        {
            context.Stock.Add(new Stock { Name = "Test", Ticker = Ticker });
            return context;
        }

        public static StockContext AddStockDividend(this StockContext context)
        {
            var stock = context.Stock.First(x => x.Ticker == Ticker);

            var fixture = new Fixture();
            fixture.Build<StockDividend>().With(sd => sd.StockId, stock.Id);
            context.AddRange(fixture.CreateMany<StockDividend>());
            return context;
        }
    }
}
