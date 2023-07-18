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

        public static StockContext AddStock(this StockContext context)
        {
            var stock = new Stock { Name = "Test", Ticker = Ticker };
            var fixture = new Fixture();
            fixture.Customize<StockDividend>(c => c.Without(sd => sd.StockId));
            fixture.Customize<StockPrice>(c => c.Without(sd => sd.StockId));
            stock.AddStockDividends(fixture.CreateMany<StockDividend>(30).ToArray());
            stock.AddPrices(fixture.CreateMany<StockPrice>().ToArray());

            context.Stock.Add(stock);
            context.SaveChanges();
            return context;
        }
    }
}
