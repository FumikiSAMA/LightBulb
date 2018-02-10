using System;
using System.Windows.Forms;

namespace LightBulb.Internal
{
    internal class WndProcEventArgs : EventArgs
    {
        public WndProcEventArgs(Message message)
        {
            Message = message;
        }

        public Message Message { get; }
    }
}