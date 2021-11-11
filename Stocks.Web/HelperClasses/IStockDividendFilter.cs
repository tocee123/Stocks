using Stocks.Core.Models;
using System;
using System.Collections.Generic;

namespace Stocks.Web.Pages
{
    public interface IStockDividendFilter
    {
        IEnumerable<Func<StockDividend, bool>> GetFilterArray();
    }
}
