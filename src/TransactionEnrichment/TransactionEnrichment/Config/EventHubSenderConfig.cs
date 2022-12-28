using System.ComponentModel.DataAnnotations;

namespace TransactionEnrichment.Config
{
    public record EventHubSenderConfig
    {
        [Required]
        public required string ConnectionString { get; init; }
        [Required]
        public required string EventHubName { get; init; }
    }
}
