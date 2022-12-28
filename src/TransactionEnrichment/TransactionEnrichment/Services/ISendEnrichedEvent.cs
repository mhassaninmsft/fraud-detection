using TransactionEnrichment.Models2;

namespace TransactionEnrichment.Services
{
    public interface ISendEnrichedEvent
    {
        Task SendEnrichedEvent(EnrichedEvent enrichedEvent);
    }
}
