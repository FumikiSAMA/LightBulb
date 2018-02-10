using System.Threading.Tasks;

namespace LightBulb.Services
{
    public interface IUpdateCheckService
    {
        Task<bool> CheckIfNewVersionAvailableAsync();
    }
}
