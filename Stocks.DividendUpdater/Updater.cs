using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stocks.Core.Loaders;
using Stocks.Core.Repositories;
using Stocks.Dal;
using Stocks.Dal.Entities;
using StockDividendCore = Stocks.Domain.Models.StockDividend;
using StockDividendEntity = Stocks.Dal.Entities.StockDividend;

namespace Stocks.DividendUpdater;
public class Updater : IUpdater
{
    readonly ILogger<Updater> _logger;
    readonly StockContext _context;
    readonly IStockDividendHistoryLoader _historyLoader;
    readonly IStocksLoader _stocksLoader;

    public Updater(ILogger<Updater> logger, StockContext context, IStockDividendHistoryLoader historyLoader, IStocksLoader stocksLoader)
    {
        _logger = logger;
        _context = context;
        _historyLoader = historyLoader;
        _stocksLoader = stocksLoader;
    }

    public async Task Update()
    {
        var stockEntities = _context.Stock.ToArray();

        var histories = await GetHistories();
        var newStockEntities = histories.Select(ToStockEntity);

        foreach (var newStock in newStockEntities)
        {
            _logger.LogInformation($"Processing {newStock.Name}");
            var firstStock = stockEntities.FirstOrDefault(s => s.Ticker == newStock.Ticker);
            if (firstStock is null)
            {
                _logger.LogInformation($"Stock {newStock.Name} wasn't found, added.");
                _context.Stock.Add(newStock);
            }
            else
            {
                AddNewStockDividendEntities(firstStock, newStock);
                AddNewStockPriceEntities(firstStock, newStock);
            }
        }

        _context.SaveChanges();
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

    async Task<IEnumerable<StockDividendCore>> GetHistories()
    {
        var tickers = _context.StockOfInterest.Select(soi => soi.Ticker);
        var histories = await _stocksLoader.GetStockDividendsAsync(tickers);
        return histories.Where(s => !string.IsNullOrEmpty(s.Ticker));
    }

    void AddNewStockDividendEntities(Stock stock, Stock newStock)
    {
        var stockDividendsEntities = _context.StockDividend.Where(sda => sda.StockId == stock.Id).ToArray();

        var newStockDividends = newStock.StockDividends.Where(sd => !stockDividendsEntities.Any(sda => sda.StockId == stock.Id && newStock.StockDividends.Select(x => x.ExDividend).ToArray().Contains(sda.ExDividend))).ToArray();
        _logger.LogInformation($"Found {newStockDividends.Length} new dividend payments for {stock.Ticker}");
        stock.AddStockDividends(newStockDividends);
    }

    void AddNewStockPriceEntities(Stock firstStock, Stock newStock)
    {
        var stockPriceEntities = _context.StockPrice.Where(sda => sda.StockId == firstStock.Id).ToArray();

        var newStockPrices = newStock.StockPrices.Where(sp => !stockPriceEntities.Any(sp => newStock.StockPrices.Select(x => x.Date).ToArray().Contains(sp.Date))).ToArray();
        _logger.LogInformation($"Found {newStockPrices.Length} new prices for {firstStock.Ticker}");
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