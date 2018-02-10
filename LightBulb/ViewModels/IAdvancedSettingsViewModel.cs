using LightBulb.Services;

namespace LightBulb.ViewModels
{
    public interface IAdvancedSettingsViewModel
    {
        ISettingsService SettingsService { get; }
    }
}