using Stocks.Core.Loaders;
using Stocks.Dal.Entities;
using StockDividendEntity = Stocks.Dal.Entities.StockDividend;
using StockDividendCore = Stocks.Domain.Models.StockDividend;
using Stocks.Core.Repositories;

var loader = new StockDividendHistoryLoader();
var stocksOfInterestRepository = new StocksOfInterestRespository();

var histories = await Task.WhenAll(stocksOfInterestRepository.GetTickers().Select(async t => await loader.DownloadStockHistoryAsync(t)));


var contextFactory = new StockContextFactory();
using var context = contextFactory.CreateDbContext(null);

context.Stock.AddRange(histories.Select(ToStockEntity));
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
    }));
    stock.AddPrice(new StockPrice { Date = DateTime.Today, Price = history.Price });
    return stock;
}