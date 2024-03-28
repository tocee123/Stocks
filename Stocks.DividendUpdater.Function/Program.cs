using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stocks.Core.Loaders;
using Stocks.Core.Updater;
using Stocks.ServiceConfiguration;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", false, true)
        .AddUserSecrets(typeof(Program).Assembly)
        .AddEnvironmentVariables()
        .Build();

        services.AddConfiguration(configuration);
        services.AddDbContext();

        services.AddTransient<IUpdater, Updater>();
        services.AddTransient<IStockDividendHistoryLoader, StockDividendHistoryLoader>();
        services.AddTransient<IStocksLoader, StocksLoader>();

        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

host.Run();




