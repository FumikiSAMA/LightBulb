using System.Threading.Tasks;
using LightBulb.Models;

namespace LightBulb.Services
{
    public interface IGeoService
    {
        Task<GeoInfo> GetGeoInfoAsync();

        Task<SolarInfo> GetSolarInfoAsync(double latitude, double longitude);
    }
}