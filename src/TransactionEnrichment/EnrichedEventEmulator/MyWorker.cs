using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EnrichedEventEmulator
{
    internal class MyWorker : BackgroundService
    {

        private readonly ILogger<MyWorker> _logger;

        public MyWorker(ILogger<MyWorker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }

    }
}
