namespace TransactionEnrichment.Models2
{
    public record DebiziumChangeEvent<T> where T : class
    {
        public Payload<T>? Payload { get; init; }
        public dynamic? Schema { get; init; }
    }
}
