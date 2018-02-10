using System;

namespace LightBulb.Services
{
    public interface IWindowService
    {
        bool IsForegroundFullScreen { get; }

        event EventHandler FullScreenStateChanged;
    }
}