using TransactionEnrichment.Models2;

namespace TransactionEnrichment.Services
{
    public interface ISendEnrichedEvent
    {
        Task SendEnrichedEvent(EnrichedEvent enrichedEvent);
        async Task SendEnrichedEvents(IEnumerable<EnrichedEvent> events)
        {
            foreach (var enrichedEvent in events)
            {
                await SendEnrichedEvent(enrichedEvent);
            }

        }
    }
}
