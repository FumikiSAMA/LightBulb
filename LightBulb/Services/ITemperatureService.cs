using System;

namespace LightBulb.Services
{
    public interface ITemperatureService
    {
        ushort Temperature { get; }

        bool IsRealtimeModeEnabled { get; set; }

        bool IsPreviewModeEnabled { get; set; }

        DateTime CyclePreviewTime { get; }

        bool IsCyclePreviewRunning { get; }

        event EventHandler Tick;

        event EventHandler Updated;

        event EventHandler CyclePreviewStarted;

        event EventHandler CyclePreviewEnded;

        void RefreshGamma();
        
        void RequestPreviewTemperature(ushort temp);

        void StartCyclePreview();

        void StopCyclePreview();
    }
}