using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using WarehouseApp.Core;
using WarehouseApp.MAUI.Services;

namespace WarehouseApp.MAUI.ViewModels;

public partial class InventoryViewModel : ObservableObject
{
    private readonly ItemService _itemService;
    private readonly INotificationService _toast;

    // Pełna lista z API
    [ObservableProperty] private ObservableCollection<Item> _items = new();

    // Lista filtrowana – na niej pracuje CollectionView
    [ObservableProperty] private ObservableCollection<Item> _filteredItems = new();

    // Tekst z SearchBar
    [ObservableProperty] private string searchText = string.Empty;
    partial void OnSearchTextChanged(string value) => FilterItems();

    public InventoryViewModel(ItemService itemService, INotificationService toast)
    {
        _itemService = itemService;
        _toast = toast;
    }

    // Konstruktor awaryjny
    public InventoryViewModel()
        : this(
            new ItemService(new HttpClient { BaseAddress = new Uri("https://testwarehouse.azurewebsites.net/api/") }),
            new NotificationService())
    { }

    /* ---------- Ładowanie ---------- */
    [RelayCommand]
    public async Task LoadAsync()
    {
        Items = new ObservableCollection<Item>(await _itemService.GetAllAsync());
        FilterItems();
    }

    /* ---------- Zmiana stanu ---------- */
    [RelayCommand] public Task IncreaseAsync(Item item) => ChangeStockAsync(item, +1);
    [RelayCommand] public Task DecreaseAsync(Item item) => ChangeStockAsync(item, -1);

    private async Task ChangeStockAsync(Item item, int delta)
    {
        bool ok = delta > 0
            ? await _itemService.AddStockAsync(item.Id, delta)
            : await _itemService.RemoveStockAsync(item.Id, -delta);

        if (ok)
        {
            item.Quantity += delta;   // aktualizacja lokalna
            FilterItems();            // odśwież widok
            await _toast.SuccessAsync($"Stan „{item.Name}”: {item.Quantity}");
        }
        else
            await _toast.ErrorAsync("Błąd aktualizacji stanu");
    }

    /* ---------- Usuwanie ---------- */
    [RelayCommand]
    public async Task DeleteAsync(Item item)
    {
        if (await _itemService.DeleteAsync(item.Id))
        {
            Items.Remove(item);
            FilterItems();
            await _toast.SuccessAsync("Usunięto produkt");
        }
        else
            await _toast.ErrorAsync("Nie udało się usunąć");
    }

    /* ---------- Filtrowanie ---------- */
    private void FilterItems()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            FilteredItems = new ObservableCollection<Item>(Items);
            return;
        }

        var lower = SearchText.ToLowerInvariant();
        var filtered = Items.Where(i =>
            (i.Name?.ToLowerInvariant().Contains(lower) ?? false) ||
            (i.Description?.ToLowerInvariant().Contains(lower) ?? false));

        FilteredItems = new ObservableCollection<Item>(filtered);
    }

    /* ---------- Alias dla starego kodu ---------- */
    public Task LoadItemsAsync() => LoadAsync();
}
