using Dzaba.Utils;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Italia.Lib
{
    public interface IHttp : IDisposable
    {
        Task<string> GetStringAsync(Uri url);
    }

    internal sealed class Http : IHttp
    {
        private readonly HttpClient http;

        public Http()
        {
            http = new HttpClient();
        }

        public void Dispose()
        {
            http?.Dispose();
        }

        public async Task<string> GetStringAsync(Uri url)
        {
            Require.NotNull(url, nameof(url));

            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                using (var resp = await http.SendAsync(request))
                {
                    resp.EnsureSuccessStatusCode();

                    return await resp.Content.ReadAsStringAsync();
                }
            }
        }
    }
}
