using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stocks.Core.Loaders;
using Stocks.Dal;
using Stocks.DividendUpdater;

internal sealed class Program
{
    static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .AddUserSecrets(typeof(Program).Assembly)
            .Build();

        var serviceCollection = new ServiceCollection();
        //this wont work here
        //serviceCollection.AddOptions<Settings>().Bind(configuration.GetSection(Settings.SectionName));
        serviceCollection.AddLogging(builder =>
        {
            builder.AddConsole();
        });
        serviceCollection.AddTransient<IUpdater, Updater>();
        serviceCollection.AddTransient<IStockDividendHistoryLoader, StockDividendHistoryLoader>();
        serviceCollection.AddTransient<IStocksLoader, StocksLoader>();
        serviceCollection.AddDbContext<StockContext>(options => options.UseSqlServer(configuration.GetConnectionString("StockWebDividendDB")));


        var updater = serviceCollection.BuildServiceProvider().GetRequiredService<IUpdater>();
        await updater.Update();
    }    
}

