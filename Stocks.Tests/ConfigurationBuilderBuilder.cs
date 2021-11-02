using Microsoft.Extensions.Configuration;

namespace Stocks.Test
{
    public class ConfigurationBuilderBuilder
    {
        public static IConfiguration Build()
        => new ConfigurationBuilder()
                .AddJsonFile($"appsettings.Development.json", optional: false)
                .Build();
    }
}
