using TransactionEnrichment.Services;
using TransactionEnrichment.Startup;

namespace TransactionEnrichedEmulator.Startup
{
    public static class Util
    {
        public static void ConfigureAppConfiguration(HostBuilderContext hostContext, IConfigurationBuilder configurationBuilder)
        {
            // Adding extra configuration files is done here
            //if (File.Exists(Path.Combine(hostContext.HostingEnvironment.ContentRootPath, "secrets.json")))
            //{
            //    _ = configurationBuilder.SetBasePath(hostContext.HostingEnvironment.ContentRootPath)
            //    .AddJsonFile("secrets.json");
            //}
            // How to get this user secrets id dynamically
            configurationBuilder.AddUserSecrets("dotnet-TransactionEnrichedEmulator-2a72c66e-85f6-482e-8315-1bbf3bc86649");
        }
        public static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            //services.AddConfigCustom<TransactionEnrichment.Config.Database>(config: hostContext.Configuration, nameof(Config.Database));
            services.AddConfigCustom<TransactionEnrichment.Config.EventHubSenderConfig>(hostContext.Configuration, nameof(TransactionEnrichment.Config.EventHubSenderConfig));
            services.AddSingleton<ISendEnrichedEvent, EventHubSender>();
            //services.AddSingleton<IGetGeoLocation, LocalGeoLocator>();
            services.AddSingleton<Emulator>();
            services.AddHostedService<Worker>();
        }
    }
}
