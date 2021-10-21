using NUnit.Framework;
using Stocks.Core.Excel;
using System.Collections.Generic;
using System.Drawing;

namespace WebDownloading.Test
{
    [TestFixture]
    public class PercentageToColorHelperTests
    {
        [TestCaseSource(typeof(GetFontColorByDividendToPriceSource), nameof(GetFontColorByDividendToPriceTestCases.Source))]
        public void GetFontColorByDividendToPrice_WhenRatioIsGiven_ReturnsColor(GetFontColorByDividendToPriceSource testCase)
        {
            var result = testCase.Ratio.GetFontColorByDividendToPrice();
            Assert.AreEqual(testCase.ExpectedColor, result);
        }

        public class GetFontColorByDividendToPriceSource
        {
            public double Ratio { get; set; }
            public Color ExpectedColor { get; set; }
        }

        private class GetFontColorByDividendToPriceTestCases
        {
            public static IEnumerable<GetFontColorByDividendToPriceSource> Source = new[] { new GetFontColorByDividendToPriceSource { Ratio = 0.021, ExpectedColor = Color.White}  };
        }
    }
}