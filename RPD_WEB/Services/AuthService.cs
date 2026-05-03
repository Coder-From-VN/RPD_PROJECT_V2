using RPD_WEB.Models.Auth;
using System.Net.Http.Json;

namespace RPD_WEB.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly LocalStorageService _storage;

        public AuthService(HttpClient http, LocalStorageService storage)
        {
            _http = http;
            _storage = storage;
        }

        public async Task<bool> Login(string username, string password)
        {
            var res = await _http.PostAsJsonAsync("/api/Auth/admin/login", new
            {
                username,
                password
            });

            if (!res.IsSuccessStatusCode)
                return false;

            var data = await res.Content.ReadFromJsonAsync<LoginResponse>();

            await _storage.SetItem("token", data.AccessToken);

            return true;
        }

        public async Task<string> GetToken()
        {
            return await _storage.GetItem("token");
        }

        public async Task Logout()
        {
            await _storage.RemoveItem("token");
        }
    }
}
