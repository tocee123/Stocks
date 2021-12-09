using Stocks.Domain.Models;
using System.Collections.Generic;

namespace Stocks.Core.Excel
{
    public interface IExcelSaver
    {
        byte[] SaveToExcel(IEnumerable<StockDividend> stockDividends);
    }
}