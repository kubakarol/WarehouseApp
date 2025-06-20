using System.Net.Http.Json;
using WarehouseApp.Core;

namespace WarehouseApp.MAUI.Services
{
    public class ItemService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:7073/api/item";

        public ItemService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Item>> GetItemsAsync()
        {
            try
            {
                var items = await _httpClient.GetFromJsonAsync<List<Item>>(BaseUrl);
                return items ?? new List<Item>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd pobierania danych: {ex.Message}");
                return new List<Item>();
            }
        }

        public async Task<bool> AddItemAsync(MultipartFormDataContent content)
        {
            try
            {
                var response = await _httpClient.PostAsync(BaseUrl, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd wysyłania: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> UpdateItemAsync(Item item)
        {
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{item.Id}", item);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
