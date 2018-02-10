using GalaSoft.MvvmLight;
using LightBulb.Services;

namespace LightBulb.ViewModels
{
    public class AdvancedSettingsViewModel : ViewModelBase, IAdvancedSettingsViewModel
    {
        public ISettingsService SettingsService { get; }

        public AdvancedSettingsViewModel(ISettingsService settingsService)
        {
            SettingsService = settingsService;
        }
    }
}