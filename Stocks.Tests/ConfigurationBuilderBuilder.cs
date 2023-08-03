using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Stocks.Core;

namespace Stocks.Test
{
    public class ConfigurationBuilderBuilder
    {
        public static IConfiguration Build()
        => new ConfigurationBuilder()
                .AddJsonFile($"appsettings.Development.json", optional: false)
                .Build();

        public static IOptions<Settings> GetOptions()
        => Options.Create(new Settings { Redis = Build().GetConnectionString("Redis") });
    }
}
