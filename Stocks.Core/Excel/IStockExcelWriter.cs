namespace Stocks.Core.Excel
{
    public interface IStockExcelWriter
    {
        Task<byte[]> WriteToExcelAsync();
    }
}