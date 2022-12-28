namespace TransactionEnrichment.Models2
{
    public record Payload<T> where T : class
    {
        public T? Before { get; init; }
        public T? After { get; init; }
    }
}
