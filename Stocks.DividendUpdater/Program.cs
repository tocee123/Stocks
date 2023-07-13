using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stocks.Core.Loaders;
using Stocks.Core.Repositories;
using Stocks.Dal.Entities;
using StockDividendCore = Stocks.Domain.Models.StockDividend;
using StockDividendEntity = Stocks.Dal.Entities.StockDividend;

var logger = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
}).CreateLogger<Program>();

logger.LogInformation("Hello");
var loader = new StockDividendHistoryLoader();
var stocksOfInterestRepository = new StocksOfInterestRespository();

var histories = (await Task.WhenAll(stocksOfInterestRepository.GetTickers().Select(async t => await loader.DownloadStockHistoryAsync(t)))).Where(s => !string.IsNullOrEmpty(s.Ticker));


var contextFactory = new StockContextFactory();
using var context = contextFactory.CreateDbContext(null);
var newStockEntities = histories.Select(ToStockEntity);
var stockEntities = context.Stock.ToArray();
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
        var stockDividendsEntities = context.StockDividend.Where(sda => sda.StockId == firstStock.Id).ToArray();

        var newStockDividends = newStock.StockDividends.Where(sd => !stockDividendsEntities.Any(sda => sda.StockId == firstStock.Id && newStock.StockDividends.Select(x => x.ExDividend).ToArray().Contains(sda.ExDividend))).ToArray();
        logger.LogInformation($"Found {newStockDividends.Length} new dividend payments for {firstStock.Ticker}");
        firstStock.AddStockDividends(newStockDividends);

        var stockPriceEntities = context.StockPrice.Where(sda => sda.StockId == firstStock.Id).ToArray();

        var newStockPrices = newStock.StockPrices.Where(sp => !stockPriceEntities.Any(sp => newStock.StockPrices.Select(x => x.Date).ToArray().Contains(sp.Date))).ToArray();
        logger.LogInformation($"Found {newStockPrices.Length} new prices for {firstStock.Ticker}");
        firstStock.AddPrices(newStockPrices);
    }
}

context.SaveChanges();

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