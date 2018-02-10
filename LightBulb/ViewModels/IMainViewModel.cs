using System;
using GalaSoft.MvvmLight.CommandWpf;
using LightBulb.Models;
using LightBulb.Services;

namespace LightBulb.ViewModels
{
    public interface IMainViewModel
    {
        ISettingsService SettingsService { get; }

        Version Version { get; }

        bool IsUpdateAvailable { get; }

        bool IsEnabled { get; set; }

        bool IsBlocked { get; }

        string StatusText { get; }

        CycleState CycleState { get; }

        double CyclePosition { get; }

        RelayCommand ShowMainWindowCommand { get; }
        RelayCommand ExitApplicationCommand { get; }
        RelayCommand AboutCommand { get; }
        RelayCommand ToggleEnabledCommand { get; }
        RelayCommand<double> DisableTemporarilyCommand { get; }
        RelayCommand DownloadNewVersionCommand { get; }
    }
}