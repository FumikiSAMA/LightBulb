namespace LightBulb.Models
{
    public class GeoInfo
    {
        public string CountryName { get; }

        public string CountryCode { get; }

        public string CityName { get; }

        public double Latitude { get; }

        public double Longitude { get; }

        public GeoInfo(string countryName, string countryCode, string cityName, double latitude, double longitude)
        {
            CountryName = countryName;
            CountryCode = countryCode;
            CityName = cityName;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}