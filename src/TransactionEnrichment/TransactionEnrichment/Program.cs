using Microsoft.Extensions.Hosting;
using static TransactionEnrichment.Startup.Util;
var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration(ConfigureAppConfiguration)
    .ConfigureServices(ConfigureServices)
    .Build();
Console.WriteLine("Happy");

host.Run();
