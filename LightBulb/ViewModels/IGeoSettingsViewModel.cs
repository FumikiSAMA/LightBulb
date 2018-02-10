using LightBulb.Services;

namespace LightBulb.ViewModels
{
    public interface IGeoSettingsViewModel
    {
        ISettingsService SettingsService { get; }

        bool IsGeoInfoSet { get; }

        string GeoInfoCountryFlagUrl { get; }
    }
}