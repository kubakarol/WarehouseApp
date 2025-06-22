using System.Net.Http.Json;
using WarehouseApp.Core;

namespace WarehouseApp.MAUI.Services
{
    public class ItemService
    {
        private readonly HttpClient _http;
        // Tylko Azure!
        private const string Base = "https://testwarehouse.azurewebsites.net/api/Item";

        public ItemService(HttpClient http) => _http = http;

        public async Task<List<Item>> GetAllAsync() =>
            await _http.GetFromJsonAsync<List<Item>>(Base) ?? new();

        public async Task<bool> AddAsync(MultipartFormDataContent body) =>
            (await _http.PostAsync(Base, body)).IsSuccessStatusCode;

        public async Task UpdateAsync(Item item) =>
            (await _http.PutAsJsonAsync($"{Base}/{item.Id}", item)).EnsureSuccessStatusCode();

        public async Task<bool> DeleteAsync(int id) =>
            (await _http.DeleteAsync($"{Base}/{id}")).IsSuccessStatusCode;

        public async Task<bool> AddStockAsync(int id, int qty) =>
            (await _http.PutAsync($"{Base}/{id}/add/{qty}", null)).IsSuccessStatusCode;

        public async Task<bool> RemoveStockAsync(int id, int qty) =>
            (await _http.PutAsync($"{Base}/{id}/remove/{qty}", null)).IsSuccessStatusCode;

        // 🔁 ALIASY do starego kodu
        public Task<bool> AddItemAsync(MultipartFormDataContent body) => AddAsync(body);
        public Task UpdateItemAsync(Item item) => UpdateAsync(item);
    }
}
