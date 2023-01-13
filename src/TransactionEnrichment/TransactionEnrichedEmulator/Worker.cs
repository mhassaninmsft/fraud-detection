using StackExchange.Redis;

namespace TransactionEnrichedEmulator
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly Emulator _emulator;
        private readonly IConnectionMultiplexer _redis;

        public Worker(ILogger<Worker> logger, Emulator emulator, IConnectionMultiplexer redis)
        {
            _logger = logger;
            _emulator = emulator;
            _redis = redis;
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

            //var redisDb = _redis.GetDatabase();
            ////var res = await redisDb.StringSetAsync("Key1", "Mohamed is cool");
            ////Console.WriteLine(res);
            //var myRes = await redisDb.StringGetAsync("Key1");
            //var res2 = myRes.ToString();
            //Console.WriteLine(myRes);
            //Console.WriteLine(res2);
            //Console.WriteLine($"I am happy for {res2}");
            ////var redisVal = new RedisValueWithExpiry("dsad", TimeSpan.FromSeconds(1));
            //var key = "MyKey";
            //for (int i = 1; i < 9;)
            //{
            //    var redisVal = new RedisValueWithExpiry(i.ToString(), TimeSpan.FromSeconds(i * 3));
            //    await redisDb.StringSetAsync(key + i.ToString(), i.ToString(), TimeSpan.FromSeconds(i * 3));
            //}
            //redisDb.Scan
            //var myTask = Task.Delay(TimeSpan.FromMinutes(1));
            //while (!myTask.IsCompleted)
            //{
            //    await foreach (var res in redisDb.SetScanAsync(key, "ds"))
            //    {

            //    }
            //}
            //redisDb.ListRightPush("myList", "dsad");
            _logger.LogInformation("Done with main thread");

        }
    }
}