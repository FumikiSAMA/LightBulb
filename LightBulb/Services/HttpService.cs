using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace LightBulb.Services
{
    public class HttpService : IHttpService, IDisposable
    {
        private readonly HttpClient _client;

        public HttpService()
        {
            var handler = new HttpClientHandler();
            if (handler.SupportsAutomaticDecompression)
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            _client = new HttpClient(handler, true);
            _client.DefaultRequestHeaders.Add("User-Agent", "LightBulb (github.com/Tyrrrz/LightBulb)");
        }

        public async Task<string> GetStringAsync(string url)
        {
            return await _client.GetStringAsync(url);
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
