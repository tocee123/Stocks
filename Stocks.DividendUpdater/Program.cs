using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stocks.Core.Loaders;
using Stocks.Core.Updater;
using Stocks.ServiceConfiguration;

internal sealed class Program
{
    static async Task Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddLogging(builder =>
        {
            builder.AddConsole();
        });

        var configuration = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json", false, true)
          .AddUserSecrets(typeof(Program).Assembly)
          .Build();

        serviceCollection.AddConfiguration(configuration);
        serviceCollection.AddDbContext();

        serviceCollection.AddTransient<IUpdater, Updater>();
        serviceCollection.AddTransient<IStockDividendHistoryLoader, StockDividendHistoryLoader>();
        serviceCollection.AddTransient<IStocksLoader, StocksLoader>();

        var updater = serviceCollection.BuildServiceProvider().GetRequiredService<IUpdater>();

        await updater.Update();
    }
}

