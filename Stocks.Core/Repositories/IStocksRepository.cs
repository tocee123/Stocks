namespace Stocks.Core.Repositories
{
    public interface IStocksRepository
    {
        Task<IEnumerable<StockDividend>> GetStocksAsync();
    }
}