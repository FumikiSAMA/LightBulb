﻿using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using LightBulb.Services;
using LightBulb.ViewModels;
using Microsoft.Practices.ServiceLocation;

namespace LightBulb
{
    public sealed class Locator
    {
        public static void Init()
        {
            if (ViewModelBase.IsInDesignModeStatic) return;

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // Services
            SimpleIoc.Default.Register<IGammaService, GammaService>();
            SimpleIoc.Default.Register<IGeoService, GeoService>();
            SimpleIoc.Default.Register<IHotkeyService, HotkeyService>();
            SimpleIoc.Default.Register<IHttpService, HttpService>();
            SimpleIoc.Default.Register<ISettingsService, SettingsService>();
            SimpleIoc.Default.Register<ITemperatureService, TemperatureService>();
            SimpleIoc.Default.Register<IUpdateCheckService, UpdateCheckService>();
            SimpleIoc.Default.Register<IWindowService, WindowService>();

            // View models
            SimpleIoc.Default.Register<IAdvancedSettingsViewModel, AdvancedSettingsViewModel>();
            SimpleIoc.Default.Register<IGeneralSettingsViewModel, GeneralSettingsViewModel>();
            SimpleIoc.Default.Register<IGeoSettingsViewModel, GeoSettingsViewModel>();
            SimpleIoc.Default.Register<IMainViewModel, MainViewModel>();

            // Load settings
            ServiceLocator.Current.GetInstance<ISettingsService>().Load();
        }

        public static void Cleanup()
        {
            // Save settings
            ServiceLocator.Current.GetInstance<ISettingsService>().Save();

            // ReSharper disable SuspiciousTypeConversion.Global
            (ServiceLocator.Current.GetInstance<IGammaService>() as IDisposable)?.Dispose();
            (ServiceLocator.Current.GetInstance<IGeoService>() as IDisposable)?.Dispose();
            (ServiceLocator.Current.GetInstance<IHotkeyService>() as IDisposable)?.Dispose();
            (ServiceLocator.Current.GetInstance<IHttpService>() as IDisposable)?.Dispose();
            (ServiceLocator.Current.GetInstance<ISettingsService>() as IDisposable)?.Dispose();
            (ServiceLocator.Current.GetInstance<ITemperatureService>() as IDisposable)?.Dispose();
            (ServiceLocator.Current.GetInstance<IUpdateCheckService>() as IDisposable)?.Dispose();
            (ServiceLocator.Current.GetInstance<IWindowService>() as IDisposable)?.Dispose();

            (ServiceLocator.Current.GetInstance<IAdvancedSettingsViewModel>() as IDisposable)?.Dispose();
            (ServiceLocator.Current.GetInstance<IGeneralSettingsViewModel>() as IDisposable)?.Dispose();
            (ServiceLocator.Current.GetInstance<IGeoSettingsViewModel>() as IDisposable)?.Dispose();
            (ServiceLocator.Current.GetInstance<IMainViewModel>() as IDisposable)?.Dispose();
            // ReSharper restore SuspiciousTypeConversion.Global
        }

        public IAdvancedSettingsViewModel AdvancedSettingsViewModel => ServiceLocator.Current.GetInstance<IAdvancedSettingsViewModel>();
        public IGeneralSettingsViewModel GeneralSettingsViewModel => ServiceLocator.Current.GetInstance<IGeneralSettingsViewModel>();
        public IGeoSettingsViewModel GeoSettingsViewModel => ServiceLocator.Current.GetInstance<IGeoSettingsViewModel>();
        public IMainViewModel MainViewModel => ServiceLocator.Current.GetInstance<IMainViewModel>();
    }
}