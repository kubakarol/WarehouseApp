using System.Net.Http.Json;
using WarehouseApp.Core;

namespace WarehouseApp.MAUI.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:7073/api/auth"; // zmień port jeśli trzeba

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/login", new User { Username = username, Password = password });
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<User>()
                : null;
        }

        public async Task<bool> RegisterAsync(User user)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/register", user);
            return response.IsSuccessStatusCode;
        }
    }
}
