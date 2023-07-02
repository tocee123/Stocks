namespace Stocks.Core.Extensions
{
    public static class StringExtensions
    {
        public static double ToDouble(this string s)
        {
            _ = double.TryParse(s, out var value);
            return value;
        }
    }
}
