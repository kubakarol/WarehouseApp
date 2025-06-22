using System.Net.Http.Json;
using WarehouseApp.Core;
using System.Text.Json;

namespace WarehouseApp.MAUI.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://testwarehouse.azurewebsites.net/api";

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            try
            {
                Console.WriteLine($"[🔁] Sending login request to {BaseUrl}/auth/login");
                Console.WriteLine($"[📦] Payload: {JsonSerializer.Serialize(new { username, password })}");

                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/auth/login", new User
                {
                    Username = username,
                    Password = password
                });

                Console.WriteLine($"[ℹ️] Response status: {response.StatusCode}");

                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[📨] Response content: {responseBody}");

                if (response.IsSuccessStatusCode)
                {
                    var user = JsonSerializer.Deserialize<User>(responseBody);
                    Console.WriteLine($"[✅] Login success for user: {user?.Username}");
                    return user;
                }

                return null;
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
                Console.WriteLine($"[📦] Payload: {JsonSerializer.Serialize(user)}");

                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/auth/register", user);

                Console.WriteLine($"[ℹ️] Response status: {response.StatusCode}");
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[📨] Response content: {responseBody}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[‼️] Exception in register: {ex.Message}");
                return false;
            }
        }
    }
}
