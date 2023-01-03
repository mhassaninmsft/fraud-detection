using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using TransactionEnrichment.Config;
using TransactionEnrichment.Models2;

namespace TransactionEnrichment.Services
{
    public class EventHubSender : ISendEnrichedEvent
    {
        private readonly ILogger _logger;
        private readonly EventHubSenderConfig _eventHubSenderConfig;
        private readonly EventHubProducerClient _eventHubProducerClient;
        public EventHubSender(ILoggerFactory loggerFactory, IOptions<EventHubSenderConfig> eventHubSenderConfig)
        {
            _eventHubSenderConfig = eventHubSenderConfig.Value;
            _eventHubProducerClient = new EventHubProducerClient(_eventHubSenderConfig.ConnectionString, _eventHubSenderConfig.EventHubName);
            _logger = loggerFactory.CreateLogger<EventHubSender>();
            _logger.LogDebug($"Event hub destination config is {_eventHubSenderConfig}");
        }
        public async Task SendEnrichedEvent(EnrichedEvent enrichedEvent)
        {
            var eventAsJson = JsonSerializer.Serialize(enrichedEvent);

            EventData data = new EventData(eventAsJson);
            try
            {
                await _eventHubProducerClient.SendAsync(new[] { data });
                _logger.LogInformation($"Sent destination event {eventAsJson} successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Caught exception when sending event hubs, {ex.Message}");
            }

        }
    }
}
