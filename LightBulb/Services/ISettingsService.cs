﻿using System;
using System.ComponentModel;
using LightBulb.Models;

namespace LightBulb.Services
{
    public interface ISettingsService : INotifyPropertyChanged
    {
        bool IsGammaPollingEnabled { get; set; }
        bool IsTemperatureSmoothingEnabled { get; set; }
        bool IsFullscreenBlocking { get; set; }
        bool IsCheckForUpdatesEnabled { get; set; }
        bool IsInternetSyncEnabled { get; set; }
        bool IsGeoInfoLocked { get; set; }
        ushort TemperatureEpsilon { get; set; }
        ushort DefaultMonitorTemperature { get; set; }
        ushort MinTemperature { get; set; }
        ushort MaxTemperature { get; set; }
        TimeSpan MaximumTemperatureSmoothingDuration { get; set; }
        TimeSpan TemperatureTransitionDuration { get; set; }
        TimeSpan TemperatureUpdateInterval { get; set; }
        TimeSpan GammaPollingInterval { get; set; }
        TimeSpan CheckForUpdatesInterval { get; set; }
        TimeSpan InternetSyncInterval { get; set; }
        TimeSpan SunriseTime { get; set; }
        TimeSpan SunsetTime { get; set; }
        GeoInfo GeoInfo { get; set; }
        Hotkey ToggleHotkey { get; set; }
        Hotkey TogglePollingHotkey { get; set; }
        Proxy Proxy { get; set; }

        void Load();

        void Save();
    }
}