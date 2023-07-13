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

var histories = (await Task.WhenAll(stocksOfInterestRepository.GetTickers().Take(10).Select(async t => await loader.DownloadStockHistoryAsync(t)))).Where(s => !string.IsNullOrEmpty(s.Ticker));


var contextFactory = new StockContextFactory();
using var context = contextFactory.CreateDbContext(null);
var newStockEntities = histories.Select(ToStockEntity);
foreach (var newStock in newStockEntities)
{
    logger.LogInformation($"Processing {newStock.Name}");
    var firstStock = context.Stock.FirstOrDefault(s => s.Ticker == newStock.Ticker);
    if (firstStock is null)
    {
        context.Stock.Add(newStock);
    }
    else
    {


        //foreach (var newStockDividend in newStock.StockDividends)
        //{
        //    if (!context.StockDividend.Any(sd => sd.StockId == firstStock.Id && sd.ExDividend == newStockDividend.ExDividend && sd.PayoutDate == newStockDividend.PayoutDate))
        //    {
        //        firstStock.AddStockDividends(newStockDividend);
        //    }
        //}
        //foreach (var newStockPrice in newStock.StockPrices)
        //{
        //    if (!context.StockPrice.Any(sd => sd.StockId == firstStock.Id && sd.Date == newStockPrice.Date))
        //    {
        //        firstStock.AddPrice(newStockPrice);
        //    }
        //}
    }
}

context.Stock.AddRange();
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
    stock.AddPrice(new StockPrice { Date = DateTime.Today, Price = history.Price });
    return stock;
}