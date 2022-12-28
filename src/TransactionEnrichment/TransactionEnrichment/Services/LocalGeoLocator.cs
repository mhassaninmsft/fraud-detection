using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System.Globalization;
using TransactionEnrichment.Models2;

namespace TransactionEnrichment.Services
{
    /// <summary>
    /// GeoLocator based on a CSV file stored on disk
    /// </summary>
    public class LocalGeoLocator : IGetGeoLocation
    {
        /// <summary>
        /// Location of the csv file
        /// </summary>
        private readonly string filePath = "./Resources/zip_lat.csv";
        private readonly IDictionary<string, GeoLocation> geoLocationsMap = new Dictionary<string, GeoLocation>();
        public LocalGeoLocator()
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<Record>();
                records.ToList().ForEach(record => geoLocationsMap[record.ZipCode] = new GeoLocation() { Latitude = record.Latitude, Longtitude = record.Longitude });
            }
        }
        public Task<GeoLocation> GetGeoLocationByZipCode(string zipCode)
        {
            return Task.FromResult(geoLocationsMap[zipCode]);
        }
        //ZIP,LAT,LNG
        private class Record
        {
            [Name("ZIP")]
            public string ZipCode { get; init; }
            [Name("LAT")]
            public double Latitude { get; init; }
            [Name("LNG")]
            public double Longitude { get; init; }
        }

    }
}
