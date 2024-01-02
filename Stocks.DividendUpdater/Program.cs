using Microsoft.Extensions.DependencyInjection;
internal sealed class Program
{
    static async Task Main(string[] args)
    {
        var serviceCollection = ServiceCollectionExtensions.SetupServiceCollection();

        var updater = serviceCollection.BuildServiceProvider().GetRequiredService<IUpdater>();

        await updater.SetIsDeleted();
        await updater.Update();
    }
}

