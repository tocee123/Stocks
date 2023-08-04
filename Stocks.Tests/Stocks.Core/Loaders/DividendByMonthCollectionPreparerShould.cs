using Stocks.Test.HelperClasses;
using System.Text;

namespace Stocks.Test.Stocks.Core.Loaders
{
    [TestFixture]
    public class DividendByMonthCollectionPreparerShould

    {
        private IDividendByMonthCollectionPreparer _target;
        private StocksRepository _stockRepository;

        [SetUp]
        public void Setup()
        {
            _stockRepository = new StocksRepository(StockContextInMemory.Create().AddStock());
            _target = new DividendByMonthCollectionPreparer(_stockRepository);
        }

        [Test]
        public void CummulativeTest2()
        {
            var data = new[] {
            new StockChampionByDividendToPriceRatio("1", "1", DateTime.Today.AddMonths(-2),1.0,20,1),
            new StockChampionByDividendToPriceRatio("2", "21", DateTime.Today.AddMonths(-1),2.0,10,1),
            new StockChampionByDividendToPriceRatio("2", "21", DateTime.Today,1.0,10,1),
            };
            LinkedList<StockChampionByDividendToPriceRatio> ll = new LinkedList<StockChampionByDividendToPriceRatio>();
            foreach (var i in data)
            {
                ll.AddLast(i);
            }

            var investment = 200.0;

            foreach (var item in data)
            {
                item.InvestedInCurrentMonth = investment;
                Console.WriteLine($"{item.ExDate.ToString("Y")} {item.Ticker} price {item.Price}$, stock volume {item.StockVolume}, dividend/share {item.Dividend}$, \t earned in month: {item.Earnings}");
            }

            var x = investment;

            var current = ll.First;
            while (current is not null)
            {
                current.Value.InvestedInCurrentMonth = x;
                x += current.Value.Earnings;
                current = current.Next;
            }
            Console.WriteLine(x);

            var sum = data.Aggregate(investment, (a, b) => { b.InvestedInCurrentMonth = a; return b.Earnings + b.InvestedInCurrentMonth; });
            Console.WriteLine($"{sum} vs {data.Sum(d => d.Earnings) + investment}");
        }

        //System.ArgumentOutOfRangeException : Year, Month, and Day parameters describe an un-representable DateTime.
        [Test, Ignore("This does not work on server")]
        public void CalculateCummulative()
        {
            Random random = new();

            string generateTicker() => ((char)('A' + (char)random.Next(0, 25))).ToString();
            double generateDividend() => random.Next(10, 100) / (double)100;
            double generatePrice() => random.Next(10, 100) / (double)10;
            var fixture = new Fixture();
            fixture.Customize<StockChampionByDividendToPriceRatio>(composer =>
                composer.With(s => s.Ticker, generateTicker)
                    .With(s => s.Dividend, generateDividend)
                    .With(s => s.Price, generatePrice));
            var data = fixture.CreateMany<StockChampionByDividendToPriceRatio>(12).ToList();
            for (int i = 0; i < data.Count; i++)
            {
                data[i] = data[i] with { ExDate = new DateTime(2021, i + 1, data[i].ExDate.Day) };
            }

            var investment = 100.0;

            foreach (var item in data)
            {
                item.InvestedInCurrentMonth = investment;
                Console.WriteLine($"{item.ExDate.ToString("Y")} {item.Ticker} price {item.Price}$, stock volume {item.StockVolume}, dividend/share {item.Dividend}$, \t earned in month: {item.Earnings}");
            }
        }

        [Test, Ignore("This does not work on server")]
        public async Task SelectTop1DividnedByMonth()
        {
            var stocks = (await _stockRepository.GetStocksAsync()).SelectMany(s => s.DividendHistories, (s, dh) => new { s.Name, dh.ExDate, DividendToPrice = Math.Round(dh.Amount / s.Price, 4), s.Price, dh.Amount, s.Ticker });

            var grouppedByMonth = stocks.GroupBy(s => new { s.ExDate.Month, s.ExDate.Year }).Select(s => new { s.Key, TopDividendPayer = s.OrderByDescending(so => so.DividendToPrice).FirstOrDefault() });
            Console.WriteLine("Monthly best ratios:");

            var investment = 1000;
            Console.WriteLine($"if you would invest {investment}$, you would get:");
            int calculateStockCountFromPrice(double price) => (int)(investment / price);
            var sum = 0.0;
            var sb = new StringBuilder();
            foreach (var item in grouppedByMonth.OrderByDescending(x => x.Key.Year).ThenByDescending(x => x.Key.Month))
            {
                var amountOfDividend = Math.Round(calculateStockCountFromPrice(item.TopDividendPayer.Price) * item.TopDividendPayer.Amount, 2);
                sum += amountOfDividend;
                sb.AppendLine($"In {item.Key.Year}/{item.Key.Month}: {item.TopDividendPayer.Name} '{item.TopDividendPayer.Ticker}' with {item.TopDividendPayer.Price}$, dividend {item.TopDividendPayer.Amount}$ ratio: {item.TopDividendPayer.DividendToPrice:0.00%} you could buy {calculateStockCountFromPrice(item.TopDividendPayer.Price)} stocks and get {amountOfDividend}$ on {item.TopDividendPayer.ExDate.ToShortDateString()}");
            }
            sb.AppendLine($"{sum}$");
            //Dont write it on the server
            //File.WriteAllText(@"c:\temp\test111.txt", sb.ToString());
        }

        [Test]
        public async Task GetMonthlyBestStocksByYear_CalculateYield()
        {
            var stockRepository = Substitute.For<IStocksRepository>();
            var target = new DividendByMonthCollectionPreparer(stockRepository);

            var thisYear = DateTime.Today.Year;
            var fixture = new Fixture().SetupFixtureToGenerateDateInCurrentYear();

            //fixture.Build<DividendHistory>().With(dh=>dh)
            var stock = fixture.Create<StockDividend>();

            stockRepository.GetStocksAsync().Returns(new[] { stock });

            var result = await target.GetMonthlyBestStocksByYear(thisYear);

            var yield = stock.DividendHistories.Sum(dh => dh.Amount) / stock.Price;

            result.First().Yield.Should().Be(yield);
        }
    }
}