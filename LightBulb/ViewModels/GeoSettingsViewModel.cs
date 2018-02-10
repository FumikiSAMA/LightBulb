using System;
using System.ComponentModel;
using GalaSoft.MvvmLight;
using LightBulb.Services;
using Tyrrrz.Extensions;

namespace LightBulb.ViewModels
{
    public class GeoSettingsViewModel : ViewModelBase, IGeoSettingsViewModel, IDisposable
    {
        public ISettingsService SettingsService { get; }

        public bool IsGeoInfoSet => SettingsService.GeoInfo != null;

        public string GeoInfoCountryFlagUrl => IsGeoInfoSet && SettingsService.GeoInfo.CountryCode.IsNotBlank()
            ? $"https://cdn2.f-cdn.com/img/flags/png/{SettingsService.GeoInfo.CountryCode.ToLowerInvariant()}.png"
            : "https://cdn2.f-cdn.com/img/flags/png/unknown.png";

        public GeoSettingsViewModel(ISettingsService settingsService)
        {
            SettingsService = settingsService;

            // Settings
            SettingsService.PropertyChanged += SettingsServicePropertyChanged;
        }

        private void SettingsServicePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(ISettingsService.GeoInfo))
            {
                RaisePropertyChanged(() => IsGeoInfoSet);
                RaisePropertyChanged(() => GeoInfoCountryFlagUrl);
            }
        }

        public void Dispose()
        {
            SettingsService.PropertyChanged -= SettingsServicePropertyChanged;
        }
    }
}