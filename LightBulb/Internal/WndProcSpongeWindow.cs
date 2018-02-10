using System;
using System.Windows.Forms;

namespace LightBulb.Internal
{
    internal sealed class WndProcSpongeWindow : NativeWindow
    {
        private readonly Action<Message> _wndProcHandler;

        public WndProcSpongeWindow(Action<Message> wndProcHandler)
        {
            _wndProcHandler = wndProcHandler;

            CreateHandle(new CreateParams());
        }

        protected override void WndProc(ref Message m)
        {
            _wndProcHandler(m);
            base.WndProc(ref m);
        }
    }
}