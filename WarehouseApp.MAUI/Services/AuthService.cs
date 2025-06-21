using System.Net.Http.Json;
using WarehouseApp.Core;

namespace WarehouseApp.MAUI.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://10.0.2.2:7073/api";

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            try
            {
                Console.WriteLine($"[🔁] Sending login request to {BaseUrl}/auth/login");

                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/auth/login", new User
                {
                    Username = username,
                    Password = password
                });

                Console.WriteLine($"[ℹ️] Response status: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<User>();
                    Console.WriteLine($"[✅] Login success for user: {user?.Username}");
                    return user;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[❌] Login failed: {error}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[‼️] Exception in login: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> RegisterAsync(User user)
        {
            try
            {
                Console.WriteLine($"[🔁] Sending register request to {BaseUrl}/auth/register");

                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/auth/register", user);

                Console.WriteLine($"[ℹ️] Response status: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("[✅] Registration successful.");
                    return true;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[❌] Registration failed: {error}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[‼️] Exception in register: {ex.Message}");
                return false;
            }
        }
    }
}
