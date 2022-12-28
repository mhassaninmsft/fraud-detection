using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;
using TransactionEnrichment.Config;
using TransactionEnrichment.Models;
using TransactionEnrichment.Models2;
using TransactionEnrichment.Services;
using static TransactionEnrichment.Util.Json;

namespace TransactionEnrichment
{
    public class Function1
    {
        private readonly ILogger _logger;
        private readonly MagicBankContext _magicBankContext;
        private readonly Transformation _transformation;
        private readonly IGetGeoLocation _geoLocator;
        private readonly ISendEnrichedEvent _enrichedEventSender;
        private readonly JsonSerializerOptions serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
            //PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public Function1(ILoggerFactory loggerFactory, MagicBankContext magicBankContext,
            IOptions<Transformation> transformationOptions, IGetGeoLocation geoLocator, ISendEnrichedEvent enrichedEventSender)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
            _magicBankContext = magicBankContext;
            _transformation = transformationOptions.Value;
            _geoLocator = geoLocator;
            _enrichedEventSender = enrichedEventSender;
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
        public void Run3([EventHubTrigger(Constants.EH_SOURCE_TOPIC, Connection = "EH_CONNECTION_STRING", ConsumerGroup = "$Default")] string[] input)
        {
            foreach (var val in input)
            {
                _logger.LogInformation(val);
                var cardTxEvent = JsonSerializer.Deserialize<DebiziumChangeEvent<CreditCardTransaction>>(val, serializeOptions);
                _logger.LogInformation($"{cardTxEvent}");
                _logger.LogInformation(JsonSerializer.Serialize(cardTxEvent?.Payload?.Before));
                var creditCardTx = cardTxEvent?.Payload?.After;
                if (creditCardTx == null)
                {
                    _logger.LogWarning("Received an empty card transaction");
                    return;
                }
                _logger.LogInformation(JsonSerializer.Serialize(creditCardTx));
                var tx = _magicBankContext.CreditCardTransactions.Where(tx => tx.Id == creditCardTx.Id).Include(x => x.PosMachine).Include(x => x.CreditCard).First();
                _logger.LogInformation($"{tx}");
                var geoLocation = _geoLocator.GetGeoLocationByZipCode(tx.PosMachine.ZipCode).Result;
                _logger.LogInformation($"Geo Location is {geoLocation}");
                var enrichedEvent = new EnrichedEvent()
                {
                    Id = tx.Id.ToString(),
                    CreditCardId = tx.CreditCardId.ToString(),
                    Latitude = geoLocation.Latitude,
                    Longtitude = geoLocation.Longtitude,
                    PosMachineId = tx.PosMachineId.ToString(),
                    TxTime = DateTime.UtcNow,
                    ZipCode = tx.PosMachine.ZipCode
                };
                _enrichedEventSender.SendEnrichedEvent(enrichedEvent).Wait();
            }

        }
    }
}
