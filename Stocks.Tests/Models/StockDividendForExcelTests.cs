using NUnit.Framework;
using Stocks.Core.Models;
using System;
using System.Linq;

[TestFixture]
public class StockDividendForExcelTests
{
    [Test]
    public void GetPropertyNames_ReturnsNotEmptyList()
    {
        var result = StockDividendForExcel.GetPropertyNames();
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Any());

        foreach (var item in result)
        {
            Console.WriteLine(item);
        }
    }
}