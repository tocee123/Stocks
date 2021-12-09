using Stocks.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stocks.Core.Loaders
{
    public interface IDividendByMonthCollectionPreparer
    {
        Task<IEnumerable<StockChampionByDividendToPriceRatio>> GetMonthlyBestStocksByYear(int year);
    }
}