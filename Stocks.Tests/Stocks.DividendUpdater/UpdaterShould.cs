using StockDividendEntity = Stocks.Dal.Entities.StockDividend;

namespace Stocks.Test.Stocks.DividendUpdater;

[TestFixture]
public class UpdaterShould
{
    [Test]
    public void ReturnAtLeastOneEntity()
    {
        var fixture = new Fixture();
        fixture.Customize<StockDividendEntity>(c => c.With(sd => sd.StockId, 111));

        var dividendsInDb = fixture.CreateMany<StockDividendEntity>().ToArray();

        var newDividends = new StockDividendEntity[dividendsInDb.Count() + 1];
        Array.Copy(dividendsInDb, newDividends, dividendsInDb.Count());
        newDividends[newDividends.Length - 1] = fixture.Create<StockDividendEntity>();

        var toBeInserted = newDividends.Where(d => dividendsInDb.All(db => db.StockId == d.StockId && db.PayoutDate != d.PayoutDate));
        toBeInserted.Count().Should().Be(1);
    }
}