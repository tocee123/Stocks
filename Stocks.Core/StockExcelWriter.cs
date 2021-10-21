using Stocks.Core.Excel;
using System.Threading.Tasks;

namespace Stocks.Core
{
    public class StockExcelWriter : IStockExcelWriter
    {
        private readonly IStocksRepository _stocksRepository;
        private readonly IExcelSaver _excelSaver;

        public StockExcelWriter(IStocksRepository stocksRepository, IExcelSaver excelSaver)
        {
            _stocksRepository = stocksRepository;
            _excelSaver = excelSaver;
        }

        public async Task<byte[]> WriteToExcelAsync()
        {
            var loadedStockDividendHistories = await _stocksRepository.GetStocks();
            return _excelSaver.SaveToExcel(loadedStockDividendHistories);
        }
    }
}
