using static TransactionEnrichedEmulator.Startup.Util;
IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(ConfigureAppConfiguration)
    .ConfigureServices(ConfigureServices)
    .Build();

host.Run();
