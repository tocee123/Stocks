using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stocks.Core.Loaders;
using Stocks.Core.Repositories;
using Stocks.Dal;
using Stocks.Dal.Entities;
using StockDividendCore = Stocks.Domain.Models.StockDividend;
using StockDividendEntity = Stocks.Dal.Entities.StockDividend;

internal sealed class Program
{
    static async Task Main(string[] args)
    {
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });

        var logger = loggerFactory.CreateLogger<Program>();

        //AddStockOfInterestIntoDb();

        var contextFactory = new StockContextFactory();
        using var context = contextFactory.CreateDbContext(null);
        var stockEntities = context.Stock.ToArray();

        var histories = await GetHistories(context, loggerFactory);
        var newStockEntities = histories.Select(ToStockEntity);

        foreach (var newStock in newStockEntities)
        {
            logger.LogInformation($"Processing {newStock.Name}");
            var firstStock = stockEntities.FirstOrDefault(s => s.Ticker == newStock.Ticker);
            if (firstStock is null)
            {
                logger.LogInformation($"Stock {newStock.Name} wasn't found, added.");
                context.Stock.Add(newStock);
            }
            else
            {
                AddNewStockDividendEntities(context, firstStock, newStock, logger);
                AddNewStockPriceEntities(context, firstStock, newStock, logger);
            }
        }

        context.SaveChanges();
    }

    static Stock ToStockEntity(StockDividendCore history)
    {
        var stock = new Stock
        {
            Name = history.Name,
            Ticker = history.Ticker,
        };
        stock.AddStockDividends(history.DividendHistories.Select(dh => new StockDividendEntity
        {
            Amount = dh.Amount,
            ExDividend = dh.ExDate,
            PayoutDate = dh.PayDate,
            Note = dh.Type
        }).ToArray());
        stock.AddPrices(new StockPrice { Date = DateTime.Today, Price = history.Price });
        return stock;
    }

    static async Task<IEnumerable<StockDividendCore>> GetHistories(StockContext context, ILoggerFactory loggerFactory)
    {
        var loader = new StockDividendHistoryLoader(loggerFactory.CreateLogger<StockDividendHistoryLoader>());
        var stockLoader = new StocksLoader(loader);
        var histories = await stockLoader.GetStockDividendsAsync(context.StockOfInterest.Select(soi => soi.Ticker));
        return histories.Where(s => !string.IsNullOrEmpty(s.Ticker));
    }

    static void AddNewStockDividendEntities(StockContext context, Stock stock, Stock newStock, ILogger log)
    {
        var stockDividendsEntities = context.StockDividend.Where(sda => sda.StockId == stock.Id).ToArray();

        var newStockDividends = newStock.StockDividends.Where(sd => !stockDividendsEntities.Any(sda => sda.StockId == stock.Id && newStock.StockDividends.Select(x => x.ExDividend).ToArray().Contains(sda.ExDividend))).ToArray();
        log.LogInformation($"Found {newStockDividends.Length} new dividend payments for {stock.Ticker}");
        stock.AddStockDividends(newStockDividends);
    }

    static void AddNewStockPriceEntities(StockContext context, Stock firstStock, Stock newStock, ILogger<Program> logger)
    {
        var stockPriceEntities = context.StockPrice.Where(sda => sda.StockId == firstStock.Id).ToArray();

        var newStockPrices = newStock.StockPrices.Where(sp => !stockPriceEntities.Any(sp => newStock.StockPrices.Select(x => x.Date).ToArray().Contains(sp.Date))).ToArray();
        logger.LogInformation($"Found {newStockPrices.Length} new prices for {firstStock.Ticker}");
        firstStock.AddPrices(newStockPrices);
    }

    static void AddStockOfInterestIntoDb()
    {
        var stocksOfInterestRepository = new StocksOfInterestRespository();
        var contextFactory = new StockContextFactory();
        using var context = contextFactory.CreateDbContext(null);
        context.StockOfInterest.AddRange(stocksOfInterestRepository.GetTickers().Select(t => new StockOfInterest { Ticker = t }));
        context.SaveChanges();
    }
}

