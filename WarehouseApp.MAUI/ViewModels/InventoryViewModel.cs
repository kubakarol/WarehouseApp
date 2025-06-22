using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using WarehouseApp.Core;
using WarehouseApp.MAUI.Services;

namespace WarehouseApp.MAUI.ViewModels;

public partial class InventoryViewModel : ObservableObject
{
    private readonly ItemService _itemService;
    private readonly INotificationService _toast;

    [ObservableProperty] private ObservableCollection<Item> _items = new();

    public InventoryViewModel(ItemService itemService, INotificationService toast)
    {
        _itemService = itemService;
        _toast = toast;
    }

    public InventoryViewModel()
        : this(new ItemService(new HttpClient { BaseAddress = new Uri("https://testwarehouse.azurewebsites.net/api/") }),
               new NotificationService())
    { }

    [RelayCommand]
    public async Task LoadAsync()
    {
        var items = await _itemService.GetAllAsync();
        Items = new ObservableCollection<Item>(items);
    }

    [RelayCommand]
    public async Task IncreaseAsync(Item item)
    {
        bool ok = await _itemService.AddStockAsync(item.Id, 1);
        if (ok)
        {
            await LoadAsync(); // ZAWSZE pobieraj najnowsze dane!
            await _toast.SuccessAsync($"Stan „{item.Name}” został zwiększony.");
        }
        else
            await _toast.ErrorAsync("Błąd aktualizacji stanu");
    }

    [RelayCommand]
    public async Task DecreaseAsync(Item item)
    {
        bool ok = await _itemService.RemoveStockAsync(item.Id, 1);
        if (ok)
        {
            await LoadAsync();
            await _toast.SuccessAsync($"Stan „{item.Name}” został zmniejszony.");
        }
        else
            await _toast.ErrorAsync("Błąd aktualizacji stanu");
    }

    [RelayCommand]
    public async Task DeleteAsync(Item item)
    {
        if (await _itemService.DeleteAsync(item.Id))
        {
            await LoadAsync();
            await _toast.SuccessAsync("Usunięto produkt");
        }
        else
            await _toast.ErrorAsync("Nie udało się usunąć");
    }

    // ALIASY
    public Task LoadItemsAsync() => LoadAsync();

    public void RefreshItem(Item item)
    {
        // już niepotrzebne w tym podejściu
    }
}
