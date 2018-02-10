using System;
using LightBulb.Models;

namespace LightBulb.Services
{
    public interface IHotkeyService
    {
        void Register(Hotkey hotkey, Action handler);

        void Unregister(Hotkey hotkey);

        void UnregisterAll();
    }
}