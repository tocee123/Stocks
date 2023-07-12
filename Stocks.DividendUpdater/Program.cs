using Stocks.Core.Loaders;
using Stocks.Dal.Entities;
using StockDividendEntity = Stocks.Dal.Entities.StockDividend;

var loader = new StockDividendHistoryLoader();
var history = await loader.DownloadStockHistoryAsync("QYLD");
var contextFactory = new StockContextFactory();
using var context = contextFactory.CreateDbContext(null);
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
stock.AddPrice(new StockPrice { Date = DateTime.Now, Price = history.Price });
context.Stock.Add(stock);
context.SaveChanges();