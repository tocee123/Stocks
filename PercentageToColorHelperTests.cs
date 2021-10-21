using NUnit.Framework;
using Stocks.Core.Excel;
using System.Collections.Generic;
using System.Drawing;

namespace WebDownloading.Test
{
    [TestFixture]
    public class PercentageToColorHelperTests
    {
        [Test, TestCaseSource(nameof(GetFontColorByDividendToPriceSource))]
        public void GetFontColorByDividendToPrice_WhenRatioIsGiven_ReturnsColor(double ratio, Color expectedColor)
        {
            var result = ratio.GetFontColorByDividendToPrice();
            Assert.AreEqual(expectedColor, result);
        }

        private static IEnumerable<TestCaseData> GetFontColorByDividendToPriceSource
        {
            get
            {
                yield return new TestCaseData(0.021, Color.White);
                yield return new TestCaseData(0.016, Color.White);
                yield return new TestCaseData(0.02, Color.White);
                yield return new TestCaseData(0.005, Color.White);
                yield return new TestCaseData(0.004, Color.White);
                yield return new TestCaseData(0.014, Color.Black);
                yield return new TestCaseData(0.006, Color.Black);
            }
        }

        [Test, TestCaseSource(nameof(GetBackgroundColorByDividendToPriceSource))]
        public void GetBackgroundColorByDividendToPrice_WhenRatioIsGiven_ReturnsColor(double ratio, Color expectedColor)
        {
            var result = ratio.GetBackgroundColorByDividendToPrice();
            Assert.AreEqual(expectedColor, result);
        }

        private static IEnumerable<TestCaseData> GetBackgroundColorByDividendToPriceSource
        {
            get
            {
                yield return new TestCaseData(0.021, Color.Green);
                yield return new TestCaseData(0.016, Color.DarkBlue);
                yield return new TestCaseData(0.02, Color.DarkBlue);
                yield return new TestCaseData(0.005, Color.Red);
                yield return new TestCaseData(0.004, Color.Red);
                yield return new TestCaseData(0.014, Color.LightBlue);
                yield return new TestCaseData(0.006, Color.Yellow);
            }
        }
    }
}