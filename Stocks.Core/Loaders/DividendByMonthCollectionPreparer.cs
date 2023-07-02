using Stocks.Core.Repositories;

namespace Stocks.Core.Loaders
{
    public class DividendByMonthCollectionPreparer : IDividendByMonthCollectionPreparer
    {
        private readonly IStocksRepository _stocksRepository;

        public DividendByMonthCollectionPreparer(IStocksRepository stocksRepository)
        {
            _stocksRepository = stocksRepository;
        }

        public async Task<IEnumerable<StockChampionByDividendToPriceRatio>> GetMonthlyBestStocksByYear(int year)
        {
            var stocks = await _stocksRepository.GetStocksAsync();
            var flattenStocks = FlattenStocks(stocks)
                .Where(s => s.ExDate.Year == year);
            var grouppedByMonth = flattenStocks.GroupBy(s => new { s.ExDate.Month, s.ExDate.Year }).Select(s => s.OrderByDescending(so => so.DividendToPrice).FirstOrDefault());

            return grouppedByMonth;
        }

        private static IEnumerable<StockChampionByDividendToPriceRatio> FlattenStocks(IEnumerable<StockDividend> stocks)
        {
            return stocks.SelectMany(s => s.DividendHistories, (s, dh) => new StockChampionByDividendToPriceRatio(s.Name, s.Ticker, dh.ExDate, Math.Round(dh.Amount / s.Price, 4), s.Price, dh.Amount));
        }
    }
}