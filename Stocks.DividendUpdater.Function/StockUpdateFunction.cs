using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Stocks.Core.Updater;

namespace Stocks.DividendUpdater.Function
{
    public class StockUpdateFunction
    {
        private readonly ILogger<StockUpdateFunction> _logger;
        private readonly IUpdater _updater;

        public StockUpdateFunction(ILogger<StockUpdateFunction> logger, IUpdater updater)
        {
            _logger = logger;
            _updater = updater;
        }

        [Function("UpdateStockDividends")]
        public async Task Run([TimerTrigger("%Schedule%")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            await _updater.Update();
            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
