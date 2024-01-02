using Stocks.Core.Extensions;
using System.Drawing;

namespace Stocks.Test.Stocks.Core.Extensions
{
    [TestFixture]
    public class PercentageToColorHelperShould
    {
        [Test, TestCaseSource(nameof(GetFontColorByDividendToPriceSource))]
        public void ReturnExpectedFontColorWhenRatioIsGiven(double ratio, Color expectedColor)
        {
            var result = ratio.GetFontColorByDividendToPrice();
            result.Should().Be(expectedColor);
        }

        [Test, TestCaseSource(nameof(GetBackgroundColorByDividendToPriceSource))]
        public void ReturnExpectedBackgroundColorWhenRatioIsGiven(double ratio, Color expectedColor)
        {
            var result = ratio.GetBackgroundColorByDividendToPrice();
            result.Should().Be(expectedColor);
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