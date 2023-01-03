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
                Id = "6",
                CreditCardId = creditCardId,
                Latitude = MAGeoLocation.Latitude,
                Longtitude = MAGeoLocation.Longtitude,
                PosMachineId = "1",
                TxTime = DateTime.UtcNow,
                ZipCode = "01519"
            };
            var event2 = event1 with { Id = "7", TxTime = DateTime.UtcNow.AddSeconds(1) };
            var event3 = event2 with { Id = "8", TxTime = DateTime.UtcNow.AddSeconds(2) };
            var event4 = event3 with { Id = "9", TxTime = DateTime.UtcNow.AddSeconds(3), }; //Longtitude = CAGeoLocation.Longtitude, Latitude = CAGeoLocation.Latitude };
            await _sendEnrichedEvent1.SendEnrichedEvents(new[] { event1, event2, event3, event4 });
            _logger.LogInformation("Done sending");

        }

    }
}
