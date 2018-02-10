using LightBulb.Models;

namespace LightBulb.Services
{
    public interface IGammaService
    {
        void SetLinear(ColorIntensity intensity);

        void RestoreDefault();
    }
}