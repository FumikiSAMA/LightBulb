using System;
using System.Collections.Generic;

namespace LightBulb.Internal
{
    internal delegate void WinEventHandler(
        IntPtr hWinEventHook, uint eventType, IntPtr hWnd,
        int idObject, int idChild, uint dwEventThread,
        uint dwmsEventTime);

    internal class WinHookManager : IDisposable
    {
        private readonly Dictionary<IntPtr, WinEventHandler> _hookHandlerDic;

        public WinHookManager()
        {
            _hookHandlerDic = new Dictionary<IntPtr, WinEventHandler>();
        }

        ~WinHookManager()
        {
            ReleaseUnmanagedResources();
        }

        public IntPtr Register(
            uint eventId, WinEventHandler handler,
            uint processId = 0, uint threadId = 0, uint flags = 0)
        {
            var handle =
                NativeMethods.SetWinEventHook(eventId, eventId, IntPtr.Zero, handler, processId, threadId, flags);
            _hookHandlerDic.Add(handle, handler);

            return handle;
        }

        public void Unregister(IntPtr handle)
        {
            NativeMethods.UnhookWinEvent(handle);
            _hookHandlerDic.Remove(handle);
        }

        public void UnregisterAll()
        {
            foreach (var handle in _hookHandlerDic.Keys)
                NativeMethods.UnhookWinEvent(handle);

            _hookHandlerDic.Clear();
        }

        private void ReleaseUnmanagedResources()
        {
            UnregisterAll();
        }

        public void Dispose()
        {
            UnregisterAll();
            GC.SuppressFinalize(this);
        }
    }
}