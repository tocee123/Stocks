using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stocks.Dal;

namespace Stocks.ServiceConfiguration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfiguration(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.Configure<DbAccess>(DbAccess.Redis, configuration.GetSection(DbAccess.Redis));
        serviceCollection.Configure<DbAccess>(DbAccess.StockWebDividendDB, configuration.GetSection(DbAccess.StockWebDividendDB));

        return serviceCollection;
    }

    public static IServiceCollection AddDbContext(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContextFactory<StockContext, StockContextFactoryWithOptions>();
        serviceCollection.AddTransient(sp =>
        {
            var cf = sp.GetRequiredService<IDbContextFactory<StockContext>>();

            var db = cf.CreateDbContext();

            return db;

        });
        return serviceCollection;
    }
}