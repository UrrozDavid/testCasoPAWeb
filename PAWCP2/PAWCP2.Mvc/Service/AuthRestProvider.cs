using System.Net.Http.Headers;
using System.Text;
using APW.Architecture;
using Microsoft.AspNetCore.Http;

namespace PAWCP2.Mvc.Service
{
    public class AuthRestProvider : IRestProvider
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IHttpClientFactory _httpFactory;

        public AuthRestProvider(IHttpContextAccessor accessor, IHttpClientFactory httpFactory)
        {
            _accessor = accessor;
            _httpFactory = httpFactory;
        }

        private HttpClient CreateClient(string endpoint)
        {
            var client = _httpFactory.CreateClient("api");
            ApplyBasic(client);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        private static string Combine(string endpoint, string? id)
            => string.IsNullOrWhiteSpace(id) ? endpoint : $"{endpoint.TrimEnd('/')}/{id}";

        private void ApplyBasic(HttpClient http)
        {
            var u = _accessor.HttpContext?.Session.GetString("BasicUser");
            var p = _accessor.HttpContext?.Session.GetString("BasicPass");
            if (string.IsNullOrEmpty(u) || string.IsNullOrEmpty(p)) return;

            var b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{u}:{p}"));
            http.DefaultRequestHeaders.Remove("Authorization");
            http.DefaultRequestHeaders.Add("Authorization", $"Basic {b64}");
        }

        public async Task<string> GetAsync(string endpoint, string? id)
        {
            var http = CreateClient(endpoint);
            var resp = await http.GetAsync(Combine(endpoint, id));
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadAsStringAsync();
        }

        public async Task<string> PostAsync(string endpoint, string content)
        {
            var http = CreateClient(endpoint);
            var resp = await http.PostAsync(endpoint, new StringContent(content, Encoding.UTF8, "application/json"));
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadAsStringAsync();
        }

        public async Task<string> PutAsync(string endpoint, string id, string content)
        {
            var http = CreateClient(endpoint);
            var resp = await http.PutAsync(Combine(endpoint, id), new StringContent(content, Encoding.UTF8, "application/json"));
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadAsStringAsync();
        }

        public async Task<string> DeleteAsync(string endpoint, string id)
        {
            var http = CreateClient(endpoint);
            var resp = await http.DeleteAsync(Combine(endpoint, id));
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadAsStringAsync();
        }
    }
}
