namespace Stocks.Core.DividendDisplay;

public static class StockNameExtensions
{
    public static string RemoveTicker(this string name)
    {
        var characterToIndex = "(";

        return name.Contains(characterToIndex)
            ? name[..name.IndexOf(characterToIndex)]
            : name;
    }

    public static string ShrinkNAme(this string name)
    {
        var stockNameMaxLength = 30;
        return name.Length > stockNameMaxLength
            ? $"{name[..stockNameMaxLength]}..."
            : name;
    }
}
