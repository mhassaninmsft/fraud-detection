using TransactionEnrichment.Models2;
using TransactionEnrichment.Services;

namespace TransactionEnrichedEmulator
{
    public class Emulator
    {
        private readonly ILogger<Emulator> _logger;
        private readonly ISendEnrichedEvent _sendEnrichedEvent1;
        private readonly string creditCardId = "1";
        private readonly string creditCardId2 = "1";
        private readonly GeoLocation MAGeoLocation = new() { Latitude = -54, Longtitude = 23 };
        private readonly GeoLocation CAGeoLocation = new() { Latitude = -70, Longtitude = 50 };
        public Emulator(ILogger<Emulator> logger, ISendEnrichedEvent sendEnrichedEvent)
        {
            _logger = logger;
            _sendEnrichedEvent1 = sendEnrichedEvent;
        }
        public async Task Emultae()
        {
            var event1 = new EnrichedEvent()
            {
                Id = "1",
                CreditCardId = creditCardId,
                Latitude = MAGeoLocation.Latitude,
                Longtitude = MAGeoLocation.Longtitude,
                PosMachineId = "1",
                TxTime = DateTime.UtcNow,
                ZipCode = "01519"
            };
            var event2 = event1 with { Id = "2", TxTime = DateTime.UtcNow.AddSeconds(1) };
            var event3 = event2 with { Id = "3", TxTime = DateTime.UtcNow.AddSeconds(2) };
            var event4 = event2 with { Id = "4", TxTime = DateTime.UtcNow.AddSeconds(10) };
            var event5 = event2 with { Id = "5", TxTime = DateTime.UtcNow.AddSeconds(20) };
            var event6 = event2 with { Id = "6", TxTime = DateTime.UtcNow.AddSeconds(30) };
            var event7 = event3 with { Id = "7", TxTime = DateTime.UtcNow.AddSeconds(50), Longtitude = CAGeoLocation.Longtitude, Latitude = CAGeoLocation.Latitude };
            await _sendEnrichedEvent1.SendEnrichedEvents(new[] { event1, event2, event3, event4, event5, event6, event7 });
            _logger.LogInformation("Done sending");

        }

    }
}
