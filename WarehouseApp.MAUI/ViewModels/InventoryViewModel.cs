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

    // ⚠️ NAZWA pola musi być _items albo items
    [ObservableProperty] private ObservableCollection<Item> _items = new();

    // ------------------------------------------------------------
    //  Konstruktory
    // ------------------------------------------------------------
    public InventoryViewModel(ItemService itemService, INotificationService toast)
    {
        _itemService = itemService;
        _toast = toast;
    }

    // awaryjny – gdy ktoś w kodzie robi new InventoryViewModel()
    public InventoryViewModel()
        : this(new ItemService(new HttpClient { BaseAddress = new Uri("https://localhost:7073/api/") }),
               new NotificationService())
    { }

    // ------------------------------------------------------------
    //  Komendy
    // ------------------------------------------------------------
    [RelayCommand]
    public async Task LoadAsync() =>
        Items = new ObservableCollection<Item>(await _itemService.GetAllAsync());

    [RelayCommand] public Task IncreaseAsync(Item item) => ChangeStockAsync(item, +1);
    [RelayCommand] public Task DecreaseAsync(Item item) => ChangeStockAsync(item, -1);

    [RelayCommand]
    public async Task DeleteAsync(Item item)
    {
        if (await _itemService.DeleteAsync(item.Id))
        {
            Items.Remove(item);
            await _toast.SuccessAsync("Usunięto produkt");
        }
        else
            await _toast.ErrorAsync("Nie udało się usunąć");
    }

    // ------------------------------------------------------------
    //  Prywatna logika
    // ------------------------------------------------------------
    private async Task ChangeStockAsync(Item item, int delta)
    {
        bool ok = delta > 0
            ? await _itemService.AddStockAsync(item.Id, delta)
            : await _itemService.RemoveStockAsync(item.Id, -delta);

        if (ok)
        {
            item.Quantity += delta;
            Items[Items.IndexOf(item)] = item;
            await _toast.SuccessAsync($"Stan „{item.Name}”: {item.Quantity}");
        }
        else
            await _toast.ErrorAsync("Błąd aktualizacji stanu");
    }

    // ------------------------------------------------------------
    //  ALIASY dla starego kodu
    // ------------------------------------------------------------
    public Task LoadItemsAsync() => LoadAsync();

    public void RefreshItem(Item item)
    {
        var ix = Items.IndexOf(item);
        if (ix >= 0) Items[ix] = item;
    }
}
