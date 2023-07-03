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

    public static Fixture SetupFixtureToGenerateDateInCurrentMonth(this Fixture fixture, int year, int month)
    {
        fixture.Customizations.Add(new RandomDateTimeSequenceGenerator(
        minDate: new DateTime(year, month, 1),
        maxDate: new DateTime(year, month % 12 + 1, 1).AddDays(-1)
        ));
        return fixture;
    }
}