namespace TransactionEnrichedEmulator
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly Emulator _emulator;

        public Worker(ILogger<Worker> logger, Emulator emulator)
        {
            _logger = logger;
            _emulator = emulator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //    await Task.Delay(1000, stoppingToken);
            //}
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await _emulator.Emultae();
            _logger.LogInformation("Done with main thread");

        }
    }
}