using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LightBulb.Internal;
using LightBulb.Models;

namespace LightBulb.Services
{
    public class WindowService : IWindowService, IDisposable
    {
        private static readonly string[] SystemClassNames =
        {
            "Progman", "WorkerW", "ImmersiveLauncher", "ImmersiveSwitchList"
        };

        private readonly WinHookManager _winHookManager;

        private IntPtr _foregroundWindowLocationChangedHook;

        private IntPtr _lastForegroundWindow;
        private bool _isForegroundFullScreen;

        public bool IsForegroundFullScreen
        {
            get => _isForegroundFullScreen;
            private set
            {
                if (IsForegroundFullScreen == value) return;

                _isForegroundFullScreen = value;
                FullScreenStateChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler FullScreenStateChanged;

        public WindowService()
        {
            _winHookManager = new WinHookManager();

            var foregroundWindowLocationChangedEventHandler = new WinEventHandler(
                (hook, type, hwnd, idObject, child, thread, time) =>
                {
                    if (idObject != 0) return; // only events from windows
                    if (hwnd != _lastForegroundWindow) return; // skip non-foreground windows

                    IsForegroundFullScreen = IsWindowFullScreen(hwnd);
                });
            var foregroundWindowChangedEventHandler = new WinEventHandler(
                (hook, type, hwnd, idObject, child, thread, time) =>
                {
                    if (idObject != 0) return; // only events from windows

                    _lastForegroundWindow = hwnd;
                    IsForegroundFullScreen = IsWindowFullScreen(hwnd);

                    // Hook location changed event for foreground window
                    if (_foregroundWindowLocationChangedHook != IntPtr.Zero)
                        _winHookManager.Unregister(_foregroundWindowLocationChangedHook);
                    _foregroundWindowLocationChangedHook = _winHookManager.Register(0x800B,
                        foregroundWindowLocationChangedEventHandler, 0, thread);
                });

            _winHookManager.Register(0x0003, foregroundWindowChangedEventHandler);

            // Init
            IsForegroundFullScreen = IsWindowFullScreen(GetForegroundWindow());
        }

        public IntPtr GetForegroundWindow()
        {
            var result = NativeMethods.GetForegroundWindow();
            return result;
        }

        public IntPtr GetDesktopWindow()
        {
            var result = NativeMethods.GetDesktopWindow();
            return result;
        }

        public IntPtr GetShellWindow()
        {
            var result = NativeMethods.GetShellWindow();
            return result;
        }

        public Rect GetWindowRect(IntPtr hWindow)
        {
            NativeMethods.GetWindowRect(hWindow, out var result);
            return result;
        }

        public Rect GetWindowClientRect(IntPtr hWindow)
        {
            NativeMethods.GetWindowClientRect(hWindow, out var result);
            return result;
        }

        public bool IsWindowVisible(IntPtr hWindow)
        {
            var result = NativeMethods.IsWindowVisible(hWindow);
            return result;
        }

        public string GetClassName(IntPtr hWindow)
        {
            var sb = new StringBuilder(256);
            NativeMethods.GetClassName(hWindow, sb, sb.Capacity);
            return sb.ToString();
        }

        public bool IsWindowFullScreen(IntPtr hWindow)
        {
            if (hWindow == IntPtr.Zero) return false;

            // Get desktop and shell
            var desktop = GetDesktopWindow();
            var shell = GetShellWindow();

            // If window is desktop or shell - return
            if (hWindow == desktop || hWindow == shell)
                return false;

            // If system window - return
            var className = GetClassName(hWindow);
            if (SystemClassNames.Contains(className, StringComparer.OrdinalIgnoreCase))
                return false;

            // If not visible - return
            if (!IsWindowVisible(hWindow))
                return false;

            // Get the window rect
            var windowRect = GetWindowRect(hWindow);

            // If window doesn't have a rect - return
            if (windowRect.Left <= 0 && windowRect.Top <= 0 && windowRect.Right <= 0 && windowRect.Bottom <= 0)
                return false;

            // Get client rect and actual rect
            var clientRect = GetWindowClientRect(hWindow);
            var actualRect = new Rect(
                windowRect.Left + clientRect.Left,
                windowRect.Top + clientRect.Top,
                windowRect.Left + clientRect.Right,
                windowRect.Top + clientRect.Bottom
            );

            // Get the screen rect and do a bounding box check
            var screenRect = Screen.FromHandle(hWindow).Bounds;
            var boundCheck = actualRect.Left <= 0 && actualRect.Top <= 0 &&
                              actualRect.Right >= screenRect.Right && actualRect.Bottom >= screenRect.Bottom;

            return boundCheck;
        }

        public void Dispose()
        {
            _winHookManager.Dispose();
        }
    }
}