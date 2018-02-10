using System;
using System.Windows.Forms;

namespace LightBulb.Internal
{
    internal sealed class SpongeWindow : NativeWindow
    {
        public SpongeWindow()
        {
            CreateHandle(new CreateParams());
        }

        public event EventHandler<WndProcEventArgs> WndProcFired;

        protected override void WndProc(ref Message m)
        {
            WndProcFired?.Invoke(this, new WndProcEventArgs(m));
            base.WndProc(ref m);
        }
    }
}