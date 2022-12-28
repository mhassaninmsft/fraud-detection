using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;
using TransactionEnrichment.Config;
using TransactionEnrichment.Models;
using TransactionEnrichment.Models2;
using static TransactionEnrichment.Util.Json;

namespace TransactionEnrichment
{
    public class Function1
    {
        private readonly ILogger _logger;
        private readonly MagicBankContext _magicBankContext;
        private readonly Transformation _transformation;
        private readonly JsonSerializerOptions serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
            //PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public Function1(ILoggerFactory loggerFactory, MagicBankContext magicBankContext,
            IOptions<Transformation> transformationOptions)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
            _magicBankContext = magicBankContext;
            _transformation = transformationOptions.Value;
        }

        [Function("Function1")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request. 23");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            _magicBankContext.PosMachines.Select(s => s.Merchant).ToList().ForEach(s => _logger.LogInformation($"{s}"));
            response.WriteString("Welcome to Azure Functions!");

            return response;
        }

        [Function("Function2")]
        public HttpResponseData Run2([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "health")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request 2.");
            _magicBankContext.PosMachines.Select(s => s.Merchant).ToList().ForEach(s => _logger.LogInformation($"{s.Name}"));


            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions! 2");

            return response;
        }

        [Function("Function3")]
        public void Run3([EventHubTrigger("ds1.public.credit_card_transaction", Connection = "EH_CONNECTION_STRING", ConsumerGroup = "$Default")] string[] input)
        {
            foreach (var val in input)
            {
                Console.WriteLine(val);
                var merchant1 = JsonSerializer.Deserialize<DebiziumChangeEvent<CreditCardTransaction>>(val, serializeOptions);
                Console.WriteLine(merchant1);
                Console.WriteLine(JsonSerializer.Serialize(merchant1?.Payload?.Before));
                Console.WriteLine(JsonSerializer.Serialize(merchant1?.Payload?.After));

            }
            _logger.LogInformation($"First Event Hubs triggered message: {input[0]}");

        }
    }
}
