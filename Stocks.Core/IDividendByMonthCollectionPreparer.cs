using Stocks.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stocks.Core
{
    public interface IDividendByMonthCollectionPreparer
    {
        Task<IEnumerable<StockChampionByDividendToPriceRatio>> GetMonthlyBestStocksByYear(int year);
    }
}