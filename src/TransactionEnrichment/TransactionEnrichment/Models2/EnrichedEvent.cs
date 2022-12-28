namespace TransactionEnrichment.Models2
{
    public record EnrichedEvent
    {
        public required string Id { get; init; }
        public required DateTime TxTime { get; init; }
        public required string PosMachineId { get; init; }
        public required double Longtitude { get; init; }
        public required double Latitude { get; init; }
        public required string CreditCardId { get; init; }
        public required string ZipCode { get; init; }

    }
}
