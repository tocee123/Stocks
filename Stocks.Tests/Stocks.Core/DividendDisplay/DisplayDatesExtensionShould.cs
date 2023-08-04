using Newtonsoft.Json;
using Stocks.Core.DividendDisplay;
using System.IO;

namespace Stocks.Test.Core.DividendDisplay;

public class DisplayDatesExtensionShould
{
    [Test]
    public void GetSecondMaximumYield()
    {
        var fixture = new Fixture();
        var month = fixture.CreateMany<IEnumerable<DisplayDay>>();

        var displayDividendHistoriesYields = month.SelectMany(m => m.SelectMany(x => x.DisplayDividendHistories).Select(x => x.Yield));

        foreach (var item in displayDividendHistoriesYields.OrderByDescending(x=>x))
        {
            Console.WriteLine(item);
        }

        var maxDividendYield = month.GetMaximumYield();
        maxDividendYield.Should().Be(displayDividendHistoriesYields.OrderByDescending(x => x).Skip(1).First() * 100);
    }

    [Test]
    public void GetSecondMaximumYield_WhenRealDataIsGiven()
    {
        var path = "Core\\DividendDisplay\\MonthJson.txt";
        var month = JsonConvert.DeserializeObject<IEnumerable<IEnumerable<DisplayDay>>>(File.ReadAllText(path));

        var displayDividendHistoriesYields = month.SelectMany(m => m.SelectMany(x => x.DisplayDividendHistories).Select(x => new { x.Yield, x.Ticker }));

        foreach (var item in displayDividendHistoriesYields.OrderByDescending(x => x.Yield).Take(10))
        {
            Console.WriteLine(item);
        }

        var maxDividendYield = month.GetMaximumYield();
        Console.WriteLine(maxDividendYield);
        maxDividendYield.Should().Be(displayDividendHistoriesYields.OrderByDescending(x => x.Yield).Skip(1).First().Yield * 100);
    }
}