namespace TransactionEnrichment.Models2
{
    public record GeoLocation
    {
        public required double Latitude { get; init; }
        public required double Longtitude { get; init; }
    }
}
