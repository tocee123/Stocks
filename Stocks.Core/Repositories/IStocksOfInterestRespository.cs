namespace Stocks.Core.Repositories
{
    public interface IStocksOfInterestRespository
    {
        IEnumerable<string> GetTickers();
    }
}