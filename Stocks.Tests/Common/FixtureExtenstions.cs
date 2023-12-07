namespace Stocks.Test.Common;
public static class FixtureExtenstions
{
    public static Fixture SetupFixtureToGenerateDateInCurrentYear(this Fixture fixture)
    {
        fixture.SetupFixtureToGenerateDateInYear(DateTime.Now.Year);
        return fixture;
    }

    public static Fixture SetupFixtureToGenerateDateInYear(this Fixture fixture, int year)
    {
        fixture.Customizations.Add(new RandomDateTimeSequenceGenerator(
        minDate: new DateTime(year, 1, 1),
        maxDate: new DateTime(year + 1, 1, 1).AddDays(-1)
        ));
        return fixture;
    }

    public static Fixture SetupFixtureToGenerateDateInCurrentMonth(this Fixture fixture)
    {
        var today = DateTime.Today;
        (var year, var month) = (today.Year, today.Month);
        fixture.Customizations.Add(new RandomDateTimeSequenceGenerator(
        minDate: new DateTime(year, month, 1),
        maxDate: new DateTime(year + (month + 1) / 12, (month + 1) % 12, 1).AddDays(-1)
        ));
        return fixture;
    }
}