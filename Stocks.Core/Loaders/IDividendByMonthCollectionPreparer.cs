namespace Stocks.Core.Loaders
{
    public interface IDividendByMonthCollectionPreparer
    {
        Task<IEnumerable<StockChampionByDividendToPriceRatio>> GetMonthlyBestStocksByYear(int year);
    }
}