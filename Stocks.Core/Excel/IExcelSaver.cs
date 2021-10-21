using Stocks.Core.Models;
using System.Collections.Generic;

namespace Stocks.Core.Excel
{
    public interface IExcelSaver
    {
        byte[] SaveToExcel(IEnumerable<StockDividend> stockDividends);
    }
}