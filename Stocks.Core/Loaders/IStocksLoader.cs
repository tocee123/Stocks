namespace Stocks.Core.Loaders
{
    public interface IStocksLoader
    {
        Task<IEnumerable<StockDividend>> GetStockDividendsAsync(IEnumerable<string> tickers);
    }
}