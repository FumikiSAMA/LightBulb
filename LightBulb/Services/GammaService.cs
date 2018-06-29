using System;
using LightBulb.Internal;
using LightBulb.Models;

namespace LightBulb.Services
{
    public class GammaService : IGammaService, IDisposable
    {
        private readonly IntPtr _dc;
        private int _gammaChannelOffset;

        public GammaService()
        {
            _dc = NativeMethods.GetDC(IntPtr.Zero);
        }

        ~GammaService()
        {
            ReleaseUnmanagedResources();
        }

        private void OffsetGammaRamp(GammaRamp ramp)
        {
            // Some drivers will not update gamma if the ramp is same as set before
            // despite the actual gamma possibly being different than it was set.
            // This method offsets the ramp slightly, enough for it to be different
            // but not enough for the change to have any visual effect.
            _gammaChannelOffset = ++_gammaChannelOffset % 5;
            ramp.Red[255] = (ushort) (ramp.Red[255] + _gammaChannelOffset);
            ramp.Green[255] = (ushort) (ramp.Green[255] + _gammaChannelOffset);
            ramp.Blue[255] = (ushort) (ramp.Blue[255] + _gammaChannelOffset);
        }

        private void SetRamp(GammaRamp ramp)
        {
            OffsetGammaRamp(ramp);
            NativeMethods.SetDeviceGammaRamp(_dc, ref ramp);
        }

        public void SetLinear(ColorIntensity intensity)
        {
            var ramp = new GammaRamp();

            for (var i = 1; i < 256; i++)
            {
                ramp.Red[i] = (ushort) (i*255*intensity.Red);
                ramp.Green[i] = (ushort) (i*255*intensity.Green);
                ramp.Blue[i] = (ushort) (i*255*intensity.Blue);
            }

            SetRamp(ramp);
        }

        public void RestoreDefault()
        {
            SetLinear(ColorIntensity.Identity);
        }

        private void ReleaseUnmanagedResources()
        {
            RestoreDefault();
            NativeMethods.ReleaseDC(IntPtr.Zero, _dc);
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }
    }
}