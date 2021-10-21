using System.Threading.Tasks;

namespace Stocks.Core
{
    public interface IStockExcelWriter
    {
        Task<byte[]> WriteToExcelAsync();
    }
}