using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stocks.Core.Loaders;
using Stocks.Dal;
using Stocks.Dal.Entities;
using StockDividendCore = Stocks.Domain.Models.StockDividend;
using StockDividendEntity = Stocks.Dal.Entities.StockDividend;

namespace Stocks.DividendUpdater.Setup;
public class Updater : IUpdater
{
    readonly ILogger<Updater> _logger;
    readonly StockContext _context;
    readonly IStocksLoader _stocksLoader;

    public Updater(ILogger<Updater> logger, StockContext context, IStocksLoader stocksLoader)
    {
        _logger = logger;
        _context = context;
        _stocksLoader = stocksLoader;
    }

    //Todo Make it nicer
    public async Task SetIsDeleted()
    {
        var stockEntities = _context.Stock.Include(s => s.StockDividends).ToArray();

        var histories = await GetHistories();
        var newStockEntities = histories.Select(ToStockEntity);

        foreach (var newStock in newStockEntities)
        {
            _logger.LogInformation($"Processing {newStock.Name}");
            var firstStock = stockEntities.FirstOrDefault(s => s.Ticker == newStock.Ticker);
            if (firstStock is null)
            {
                continue;
            }
            MarkIsDeleted(firstStock, newStock);
        }

        _context.SaveChanges();
    }

    public async Task Update()
    {
        var stockEntities = _context.Stock.Include(s => s.StockDividends).ToArray();

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
                UpdateNewStockDividendEntities(firstStock, newStock);
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

    private void MarkIsDeleted(Stock stockInDb, Stock stockFromNet)
    {
        foreach (var dividendInDb in stockInDb.StockDividends.Except(stockFromNet.StockDividends)
            .Where(s => !s.IsDeleted))
        {
            dividendInDb.IsDeleted = true;
        }
    }

    void UpdateNewStockDividendEntities(Stock stockInDb, Stock stockFromNet)
    {
        foreach (var dividend in stockInDb.StockDividends)
        {
            var dividendFromNet = stockFromNet.StockDividends.FirstOrDefault(s => s.ExDividend == dividend.ExDividend);
            if (dividendFromNet is null)
            {
                continue;
            }
            dividend.Note = dividendFromNet.Note;
            dividend.Amount = dividendFromNet.Amount;
        }

        var stockDividendsEntities = _context.StockDividend.Where(sda => sda.StockId == stockInDb.Id).ToArray();

        var newStockDividends = stockFromNet.StockDividends.Where(newDividend => stockDividendsEntities.All(existingDividend => existingDividend.StockId == stockInDb.Id && (existingDividend.PayoutDate != newDividend.PayoutDate || existingDividend.IsDeleted)))
            .ToArray();

        _logger.LogInformation($"Found {newStockDividends.Length} new dividend payments for {stockInDb.Ticker}");
        stockInDb.AddStockDividends(newStockDividends);
    }

    void AddNewStockPriceEntities(Stock firstStock, Stock newStock)
    {
        var stockPriceEntities = _context.StockPrice.Where(sda => sda.StockId == firstStock.Id).ToArray();

        var newStockPrices = newStock.StockPrices.Where(sp => !stockPriceEntities.Any(sp => newStock.StockPrices.Select(x => x.Date).ToArray().Contains(sp.Date))).ToArray();
        _logger.LogInformation($"Found {newStockPrices.Length} new prices for {firstStock.Ticker}");
        firstStock.AddPrices(newStockPrices);
    }
}