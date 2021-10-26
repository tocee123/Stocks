using System;

namespace Stocks.Web.Pages
{
    public static class Common
    {
        public const string SwitchToUpcoming = nameof(SwitchToUpcoming);
        public const string SwitchToGraterThan1 = nameof(SwitchToGraterThan1);
        public const string HasSpecial = nameof(HasSpecial);
        public const int ZeroDays = 0;
        public const int TwoWeeks = 14;
        public const double OnePercent = 0.01;

        public static string GetCssForDividendToPrice(double dividendToPriceRatio)
        => dividendToPriceRatio switch
        {
            var n when n > 0.02 => "p-3 mb-2 bg-success text-white",
            var n when n >= 0.015 && n <= 0.02 => "p-3 mb-2 bg-primary text-white",
            var n when n > 0.01 && n < 0.015 => "p-3 mb-2 bg-info text-white",
            var n when (n > 0.005 && n <= 0.01) => "p-3 mb-2 bg-warning text-dark",
            var n when n <= 0.005 => "p-3 mb-2 bg-danger text-white",
            _ => ""
        };

        public static string GetCssForWhenToBuy(DateTime whenToBuy)
        => (whenToBuy - DateTime.Today).Days switch
        {
            var n when n > 3 && n < 7 => "p-3 mb-2 bg-success text-white",
            var n when n >= 7 && n < 14 => "p-3 mb-2 bg-warning text-dark",
            var n when n > 14 => "p-3 mb-2 bg-secondary text-white",
            var n when n <= 3 && n >= 0 => "p-3 mb-2 bg-danger text-white",
            _ => "p-3 mb-2 bg-secondary text-white"
        };

        public static string ToPercentageDisplay(this double value)
       => $"{value:0.00%}";

        public static string ToYyyyMmDd(this DateTime dt)
            => dt.ToString("yyyy-MM-dd");

        public static double Round(this double number)
            => Math.Round(number, 2);
    }
}
