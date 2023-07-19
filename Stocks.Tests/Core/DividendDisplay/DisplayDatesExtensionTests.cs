using Stocks.Core.DividendDisplay;

namespace Stocks.Test.Core.DividendDisplay;

public class DisplayDatesExtensionTests
{
    [Test]
    public void GetMaximumYield_ReturnsTheMaximumYield()
    {
        var fixture = new Fixture();
        var month = fixture.CreateMany<IEnumerable<DisplayDay>>();

        var displayDividendHistoriesYields = month.SelectMany(m => m.SelectMany(x => x.DisplayDividendHistories).Select(x => x.Yield));

        foreach (var item in displayDividendHistoriesYields.OrderByDescending(x=>x))
        {
            Console.WriteLine(item);
        }

        var maxDividendYield = month.GetMaximumYield();
        maxDividendYield.Should().Be(displayDividendHistoriesYields.OrderByDescending(x => x).First() * 100);
    }
}