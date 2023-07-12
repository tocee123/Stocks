using Stocks.Core.Loaders;
using Stocks.Dal.Entities;

var loader = new StockDividendHistoryLoader();
var history = await loader.DownloadStockHistoryAsync("QYLD");
var contextFactory = new StockContextFactory();
using var context = contextFactory.CreateDbContext(null);
var stock = new Stock { 
Name = history.Name,
Ticker = history.Ticker,
StockDividends = history.
};