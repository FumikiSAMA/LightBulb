using System.Threading.Tasks;

namespace LightBulb.Services
{
    public interface IHttpService
    {
        Task<string> GetStringAsync(string url);
    }
}