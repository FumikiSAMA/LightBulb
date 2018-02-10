using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;
using LightBulb.Internal;
using LightBulb.Models;
using Tyrrrz.Extensions;

namespace LightBulb.Services
{
    public class HotkeyService : IHotkeyService, IDisposable
    {
        private readonly WndProcSpongeWindow _wndProcSpongeWindow;
        private readonly Dictionary<int, Action> _hotkeyHandlerDic;

        public HotkeyService()
        {
            _wndProcSpongeWindow = new WndProcSpongeWindow(ProcessMessage);
            _hotkeyHandlerDic = new Dictionary<int, Action>();
        }

        ~HotkeyService()
        {
            ReleaseUnmanagedResources();
        }

        private void ProcessMessage(Message message)
        {
            if (message.Msg != 0x0312) return;

            var id = message.WParam.ToInt32();
            var handler = _hotkeyHandlerDic.GetOrDefault(id);

            handler?.Invoke();
        }

        public void Register(Hotkey hotkey, Action handler)
        {
            var vk = KeyInterop.VirtualKeyFromKey(hotkey.Key);
            var mods = (int) hotkey.Modifiers;
            var id = (vk << 8) | mods;

            NativeMethods.RegisterHotKey(_wndProcSpongeWindow.Handle, id, mods, vk);
            _hotkeyHandlerDic.Add(id, handler);
        }

        public void Unregister(Hotkey hotkey)
        {
            var vk = KeyInterop.VirtualKeyFromKey(hotkey.Key);
            var mods = (int) hotkey.Modifiers;
            var id = (vk << 8) | mods;

            NativeMethods.UnregisterHotKey(_wndProcSpongeWindow.Handle, id);
            _hotkeyHandlerDic.Remove(id);
        }

        public void UnregisterAll()
        {
            foreach (var id in _hotkeyHandlerDic.Keys)
                NativeMethods.UnregisterHotKey(_wndProcSpongeWindow.Handle, id);

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