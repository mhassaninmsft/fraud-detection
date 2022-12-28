using TransactionEnrichment.Models2;

namespace TransactionEnrichment.Services
{
    public interface IGetGeoLocation
    {
        Task<GeoLocation> GetGeoLocationByZipCode(string zipCode);
    }
}
