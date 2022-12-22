using CreditCardSimulator.Database;
using Microsoft.EntityFrameworkCore;


namespace CreditCardSimulator.Startup
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
            //configurationBuilder.AddUserSecrets(;
        }
        public static void ConfigureDatabase(HostBuilderContext hostContext, IServiceCollection services)
        {
            IConfiguration config = hostContext.Configuration;
            var dbConfig = config.GetSection(nameof(Config.Database)).Get<Config.Database>();
            services.AddDbContext<Models.MagicBankContext>(
                   options => options.UseNpgsql(dbConfig.ConnectionString));

        }
        public static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddConfigCustom<Config.Database>(config: hostContext.Configuration, nameof(Config.Database));
            ConfigureDatabase(hostContext, services);
            services.AddScoped<Functions>();
            // Add Odata
            //services.AddOData(opt => opt.AddRouteComponents("odata", GetEdmModel()));
            //IConfiguration config = hostContext.Configuration;
            //var appConfig = config.GetSection(nameof(AzureMaps)).Get<AzureMaps>();
            //Console.WriteLine(appConfig);
            //_ = services.AddConfigCustom<AzureMaps>(config, nameof(AzureMaps));
            //services.AddSingleton<IMapsService, AzureMapsService>();
            //services.AddHttpClient(Constants.AzureMapsHttpClientName, client =>
            //{
            //    client.BaseAddress = appConfig.Endpoint;
            //    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", appConfig.SubscriptionKey);
            //}).AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
            //    {
            //        TimeSpan.FromSeconds(1),
            //        TimeSpan.FromSeconds(5),
            //        TimeSpan.FromSeconds(10),
            //    }));
        }
        public static IServiceCollection AddConfigCustom<T>(
            this IServiceCollection services,
            IConfiguration config,
            string sectionName,
            Func<T, bool>? customValidator = null)
            where T : class
        {
            customValidator ??= (configClass) => true;
            _ = services.AddOptions<T>()
                   .Bind(config.GetSection(sectionName), binderOptions =>
                   {
                       binderOptions.ErrorOnUnknownConfiguration = true;
                   })
               .ValidateDataAnnotations()
               .Validate(customValidator)
               .ValidateOnStart();
            return services;

        }
    }
}
