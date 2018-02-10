using System;
using System.Collections.Generic;
using System.Diagnostics;

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

        public IntPtr RegisterWinEvent(
            uint eventId, WinEventHandler handler,
            uint processId = 0, uint threadId = 0, uint flags = 0)
        {
            var handle = NativeMethods.SetWinEventHook(eventId, eventId, IntPtr.Zero, handler, processId, threadId, flags);
            if (handle == IntPtr.Zero)
            {
                Debug.WriteLine($"Could not register WinEventHook for {eventId}", GetType().Name);
                return IntPtr.Zero;
            }

            _hookHandlerDic.Add(handle, handler);
            return handle;
        }

        public void UnregisterWinEvent(IntPtr handle)
        {
            if (!NativeMethods.UnhookWinEvent(handle))
            {
                Debug.WriteLine("Could not unregister WinEventHook", GetType().Name);
            }

            _hookHandlerDic.Remove(handle);
        }

        public void UnregisterAllWinEvents()
        {
            foreach (var hook in _hookHandlerDic)
            {
                if (!NativeMethods.UnhookWinEvent(hook.Key))
                {
                    Debug.WriteLine("Could not unregister WinEventHook", GetType().Name);
                }
            }

            _hookHandlerDic.Clear();
        }

        private void ReleaseUnmanagedResources()
        {
            UnregisterAllWinEvents();
        }

        public void Dispose()
        {
            UnregisterAllWinEvents();
            GC.SuppressFinalize(this);
        }
    }
}