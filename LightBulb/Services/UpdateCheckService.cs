using System;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace LightBulb.Services
{
    public class UpdateCheckService : IUpdateCheckService
    {
        private readonly IHttpService _httpService;

        public UpdateCheckService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<bool> CheckIfNewVersionAvailableAsync()
        {
            var response = await _httpService.GetStringAsync("https://api.github.com/repos/Tyrrrz/LightBulb/releases");

            // Get current version
            var currentVersion = Assembly.GetEntryAssembly().GetName().Version;

            // Parse
            var releasesJson = JToken.Parse(response);

            // Check versions of all releases, see if any one of them is newer than the current
            foreach (var releaseJson in releasesJson)
            {
                var tagName = releaseJson["tag_name"].Value<string>();

                if (Version.TryParse(tagName, out var version) && currentVersion < version)
                    return true;
            }

            return false;
        }
    }
}