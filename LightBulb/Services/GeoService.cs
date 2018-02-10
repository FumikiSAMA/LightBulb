using System;
using System.Globalization;
using System.Threading.Tasks;
using LightBulb.Models;
using Newtonsoft.Json.Linq;
using Tyrrrz.Extensions;

namespace LightBulb.Services
{
    public class GeoService : IGeoService
    {
        private readonly IHttpService _httpService;

        public GeoService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<GeoInfo> GetGeoInfoAsync()
        {
            var response = await _httpService.GetStringAsync("http://freegeoip.net/json");

            // Parse
            var geoInfoJson = JToken.Parse(response);

            // Extract data
            var countryName = geoInfoJson["country_name"].Value<string>().NullIfBlank();
            var countryCode = geoInfoJson["country_code"].Value<string>().NullIfBlank();
            var city = geoInfoJson["city"].Value<string>().NullIfBlank();
            var lat = geoInfoJson["latitude"].Value<double>();
            var lng = geoInfoJson["longitude"].Value<double>();

            // Populate
            var result = new GeoInfo(countryName, countryCode, city, lat, lng);

            return result;
        }

        public async Task<SolarInfo> GetSolarInfoAsync(double latitude, double longitude)
        {
            var latStr = latitude.ToString(CultureInfo.InvariantCulture);
            var lngStr = longitude.ToString(CultureInfo.InvariantCulture);
            var url = $"http://api.sunrise-sunset.org/json?lat={latStr}&lng={lngStr}&formatted=0";
            var response = await _httpService.GetStringAsync(url);

            // Parse
            var solarInfoJson = JToken.Parse(response);

            // Extract data
            var sunrise = solarInfoJson["results"]["sunrise"].Value<DateTime>();
            var sunset = solarInfoJson["results"]["sunset"].Value<DateTime>();

            // Populate
            var result = new SolarInfo(sunrise.TimeOfDay, sunset.TimeOfDay);

            return result;
        }
    }
}