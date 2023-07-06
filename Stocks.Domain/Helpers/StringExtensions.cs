namespace Stocks.Domain.Helpers
{
    public static class StringExtensions
    {
        public static string ToPercentageDisplay(this double value)
           => $"{value:0.00%}";
        public static string ToDollarDisplay(this double value)
        => $"${value:0.00}";

    }
}
