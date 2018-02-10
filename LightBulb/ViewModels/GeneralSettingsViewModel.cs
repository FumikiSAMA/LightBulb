using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using LightBulb.Services;

namespace LightBulb.ViewModels
{
    public class GeneralSettingsViewModel : ViewModelBase, IGeneralSettingsViewModel, IDisposable
    {
        private readonly ITemperatureService _temperatureService;

        public ISettingsService SettingsService { get; }

        public bool IsPreviewModeEnabled
        {
            get => _temperatureService.IsPreviewModeEnabled;
            set => _temperatureService.IsPreviewModeEnabled = value;
        }

        public bool IsCyclePreviewRunning
        {
            get => _temperatureService.IsCyclePreviewRunning;
            set
            {
                if (value)
                    _temperatureService.StartCyclePreview();
                else
                    _temperatureService.StopCyclePreview();
            }
        }

        public RelayCommand StartStopCyclePreviewCommand { get; }
        public RelayCommand<ushort> RequestPreviewTemperatureCommand { get; }

        public GeneralSettingsViewModel(ITemperatureService temperatureService, ISettingsService settingsService)
        {
            // Services
            SettingsService = settingsService;
            _temperatureService = temperatureService;

            _temperatureService.CyclePreviewStarted += TemperatureServiceCyclePreviewStarted;
            _temperatureService.CyclePreviewEnded += TemperatureServiceCyclePreviewEnded;

            // Commands
            RequestPreviewTemperatureCommand = new RelayCommand<ushort>(RequestPreviewTemperature);
            StartStopCyclePreviewCommand = new RelayCommand(StartStopCyclePreview);
        }

        private void TemperatureServiceCyclePreviewStarted(object sender, EventArgs args)
        {
            RaisePropertyChanged(() => IsCyclePreviewRunning);
        }

        private void TemperatureServiceCyclePreviewEnded(object sender, EventArgs args)
        {
            RaisePropertyChanged(() => IsCyclePreviewRunning);
        }

        private void RequestPreviewTemperature(ushort temp)
        {
            _temperatureService.RequestPreviewTemperature(temp);
        }

        private void StartStopCyclePreview()
        {
            IsCyclePreviewRunning = !IsCyclePreviewRunning;
        }

        public void Dispose()
        {
            _temperatureService.CyclePreviewStarted -= TemperatureServiceCyclePreviewStarted;
            _temperatureService.CyclePreviewEnded -= TemperatureServiceCyclePreviewEnded;
        }
    }
}