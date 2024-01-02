using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stocks.Core.Loaders;
using Stocks.Dal;
using Stocks.DividendUpdater.Setup;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection SetupServiceCollection()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddLogging(builder =>
        {
            builder.AddConsole();
        });

        serviceCollection.AddConfiguration();
        serviceCollection.AddDbContext();
        serviceCollection.AddDependenxies();

        return serviceCollection;
    }

    private static IServiceCollection AddConfiguration(this IServiceCollection serviceCollection)
    {
        var configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json", false, true)
           .AddUserSecrets(typeof(Program).Assembly)
           .Build();

        serviceCollection.Configure<DbAccess>(DbAccess.Redis, configuration.GetSection(DbAccess.Redis));
        serviceCollection.Configure<DbAccess>(DbAccess.StockWebDividendDB, configuration.GetSection(DbAccess.StockWebDividendDB));

        return serviceCollection;
    }

    private static IServiceCollection AddDbContext(this IServiceCollection serviceCollection)
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

    private static IServiceCollection AddDependenxies(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IUpdater, Updater>();
        serviceCollection.AddTransient<IStockDividendHistoryLoader, StockDividendHistoryLoader>();
        serviceCollection.AddTransient<IStocksLoader, StocksLoader>();

        return serviceCollection;
    }
}

