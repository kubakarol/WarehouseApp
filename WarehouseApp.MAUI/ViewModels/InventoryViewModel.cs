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


    [ObservableProperty] private ObservableCollection<Item> _items = new();

    [ObservableProperty] private ObservableCollection<Item> _filteredItems = new();

    [ObservableProperty] private string searchText = string.Empty;
    partial void OnSearchTextChanged(string value) => FilterItems();

    public InventoryViewModel(ItemService itemService, INotificationService toast)
    {
        _itemService = itemService;
        _toast = toast;
    }

    public InventoryViewModel()
        : this(
            new ItemService(new HttpClient { BaseAddress = new Uri("https://testwarehouse.azurewebsites.net/api/") }),
            new NotificationService())
    { }

    [RelayCommand]
    public async Task LoadAsync()
    {
        Items = new ObservableCollection<Item>(await _itemService.GetAllAsync());
        FilterItems();
    }

    [RelayCommand] public Task IncreaseAsync(Item item) => ChangeStockAsync(item, +1);
    [RelayCommand] public Task DecreaseAsync(Item item) => ChangeStockAsync(item, -1);

    private async Task ChangeStockAsync(Item item, int delta)
    {
        bool ok = delta > 0
            ? await _itemService.AddStockAsync(item.Id, delta)
            : await _itemService.RemoveStockAsync(item.Id, -delta);

        if (ok)
        {
            item.Quantity += delta;   
            FilterItems();            
            await _toast.SuccessAsync($"Stan „{item.Name}”: {item.Quantity}");
        }
        else
            await _toast.ErrorAsync("Błąd aktualizacji stanu");
    }

 
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


    public Task LoadItemsAsync() => LoadAsync();
}
