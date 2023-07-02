namespace Stocks.Core.Excel
{
    public interface IExcelSaver
    {
        byte[] SaveToExcel(IEnumerable<StockDividend> stockDividends);
    }
}