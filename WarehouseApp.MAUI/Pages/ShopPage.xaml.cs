using Microsoft.Maui.Controls;
using WarehouseApp.Core;
using WarehouseApp.MAUI.Services;
using WarehouseApp.MAUI.ViewModels;

namespace WarehouseApp.MAUI.Pages;

/// <summary>
/// Strona sklepu (lista produktów + dodawanie do koszyka)
/// </summary>
public partial class ShopPage : ContentPage
{
    private readonly InventoryViewModel _inventoryVm;

    // ViewModel wstrzyknięty z DI – ten sam, którego używa InventoryPage
    public ShopPage(InventoryViewModel inventoryVm)
    {
        InitializeComponent();
        _inventoryVm = inventoryVm;
        BindingContext = _inventoryVm;
    }

    /* ------------------- Dodawanie do koszyka ------------------- */
    private async void OnAddToCartClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is Item item)
        {
            if (item.Quantity <= 0)
            {
                await DisplayAlert("Brak towaru", "Nie ma więcej sztuk tego produktu.", "OK");
                return;
            }

            // Dodanie (lub zwiększenie) pozycji w koszyku
            var existing = CartStore.Cart.FirstOrDefault(c => c.Item.Id == item.Id);
            if (existing.Item != null)
            {
                int ix = CartStore.Cart.FindIndex(c => c.Item.Id == item.Id);
                CartStore.Cart[ix] = (existing.Item, existing.Count + 1);
            }
            else
            {
                CartStore.Cart.Add((item, 1));
            }

            try { Vibration.Default.Vibrate(100); } catch { }

            // 🔄 pobierz świeży stan magazynu (Quantity zaktualizowany w API)
            await _inventoryVm.LoadAsync();

            await DisplayAlert("Koszyk", $"Dodano: {item.Name}", "OK");
        }
    }

    /* ------------------- Nawigacja ------------------- */
    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        Preferences.Set("Role", string.Empty);
        await Shell.Current.GoToAsync("//LoginPage");
    }

    private async void OnCartClicked(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PushAsync(new CartPage());
    }

    /* ------------------- Życie strony ------------------- */
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _inventoryVm.LoadAsync();   // zawsze świeże dane przy wejściu
    }
}
