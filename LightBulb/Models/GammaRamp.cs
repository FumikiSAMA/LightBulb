using System.Runtime.InteropServices;

namespace LightBulb.Models
{
    [StructLayout(LayoutKind.Sequential)]
    public class GammaRamp
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        private readonly ushort[] _red;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        private readonly ushort[] _green;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        private readonly ushort[] _blue;

        public ushort[] Red => _red;

        public ushort[] Green => _green;

        public ushort[] Blue => _blue;

        public GammaRamp()
        {
            _red = new ushort[256];
            _green = new ushort[256];
            _blue = new ushort[256];
        }
    }
}