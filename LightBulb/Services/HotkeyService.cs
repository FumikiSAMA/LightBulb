using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using LightBulb.Internal;
using LightBulb.Models;
using Tyrrrz.Extensions;

namespace LightBulb.Services
{
    public class HotkeyService : IHotkeyService, IDisposable
    {
        private readonly SpongeWindow _sponge;
        private readonly Dictionary<int, Action> _hotkeyHandlerDic;

        public HotkeyService()
        {
            _sponge = new SpongeWindow();
            _hotkeyHandlerDic = new Dictionary<int, Action>();

            _sponge.WndProcFired += ProcessMessage;
        }

        ~HotkeyService()
        {
            ReleaseUnmanagedResources();
        }

        private void ProcessMessage(object sender, WndProcEventArgs args)
        {
            if (args.Message.Msg != 0x0312) return;

            var id = args.Message.WParam.ToInt32();
            var handler = _hotkeyHandlerDic.GetOrDefault(id);

            handler?.Invoke();
        }

        public void Register(Hotkey hotkey, Action handler)
        {
            var vk = KeyInterop.VirtualKeyFromKey(hotkey.Key);
            var mods = (int) hotkey.Modifiers;
            var id = (vk << 8) | mods;

            if (!NativeMethods.RegisterHotKey(_sponge.Handle, id, mods, vk))
            {
                Debug.WriteLine("Could not register a hotkey", GetType().Name);
                return;
            }

            _hotkeyHandlerDic.Add(id, handler);
        }

        public void Unregister(Hotkey hotkey)
        {
            var vk = KeyInterop.VirtualKeyFromKey(hotkey.Key);
            var mods = (int) hotkey.Modifiers;
            var id = (vk << 8) | mods;

            if (!NativeMethods.UnregisterHotKey(_sponge.Handle, id))
            {
                Debug.WriteLine("Could not unregister a hotkey", GetType().Name);
            }

            _hotkeyHandlerDic.Remove(id);
        }

        public void UnregisterAll()
        {
            foreach (var hotkey in _hotkeyHandlerDic)
            {
                if (!NativeMethods.UnregisterHotKey(_sponge.Handle, hotkey.Key))
                {
                    Debug.WriteLine("Could not unregister a hotkey", GetType().Name);
                }
            }

            _hotkeyHandlerDic.Clear();
        }

        private void ReleaseUnmanagedResources()
        {
            UnregisterAll();
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }
    }
}