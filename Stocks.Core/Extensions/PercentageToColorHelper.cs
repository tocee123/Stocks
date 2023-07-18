using System.Drawing;

namespace Stocks.Core.Extensions
{
    public static class PercentageToColorHelper
    {
        public static Color GetFontColorByDividendToPrice(this double dividendToPrice)
            => dividendToPrice > 0.02
            || dividendToPrice >= 0.015 && dividendToPrice <= 0.02
            || dividendToPrice <= 0.005
            ? Color.White
            : Color.Black;

        public static Color GetBackgroundColorByDividendToPrice(this double dividendToPrice)
        => dividendToPrice switch
        {
            var n when n > 0.02 => Color.Green,
            var n when n >= 0.015 && n <= 0.02 => Color.DarkBlue,
            var n when n > 0.01 && n < 0.015 => Color.LightBlue,
            var n when n > 0.005 && n <= 0.01 => Color.Yellow,
            var n when n <= 0.005 => Color.Red,
            _ => Color.White
        };
    }
}
