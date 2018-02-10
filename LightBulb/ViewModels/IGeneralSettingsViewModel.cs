using GalaSoft.MvvmLight.CommandWpf;
using LightBulb.Services;

namespace LightBulb.ViewModels
{
    public interface IGeneralSettingsViewModel
    {
        ISettingsService SettingsService { get; }

        bool IsPreviewModeEnabled { get; set; }

        bool IsCyclePreviewRunning { get; set; }

        RelayCommand StartStopCyclePreviewCommand { get; }
    }
}