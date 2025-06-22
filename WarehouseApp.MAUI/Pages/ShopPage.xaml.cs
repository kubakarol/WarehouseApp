using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;          // Vibration
using WarehouseApp.Core;
using WarehouseApp.MAUI.Services;      // ←★ CartStore tutaj!
using WarehouseApp.MAUI.ViewModels;

namespace WarehouseApp.MAUI.Pages;

public partial class ShopPage : ContentPage
{
    private readonly InventoryViewModel _inventoryVm;

    // Konstruktor wołany przez Shell
    public ShopPage() : this(Resolve<InventoryViewModel>()) { }

    // Główny konstruktor (DI)
    public ShopPage(InventoryViewModel vm)
    {
        InitializeComponent();
        _inventoryVm = vm;
        BindingContext = _inventoryVm;
    }

    /* ---------- Handlery UI ---------- */

    private async void OnAddToCartClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is Item item)
        {
            if (item.Quantity <= 0)
            {
                await DisplayAlert("Brak towaru", "Nie ma więcej sztuk.", "OK");
                return;
            }

            // dodaj / zwiększ w koszyku
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

            await _inventoryVm.LoadAsync();
            await DisplayAlert("Koszyk", $"Dodano: {item.Name}", "OK");
        }
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        Preferences.Set("Role", string.Empty);
        await Shell.Current.GoToAsync("//LoginPage");
    }

    private async void OnCartClicked(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PushAsync(new CartPage());
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _inventoryVm.LoadAsync();
    }

    /* ---------- Helper DI ---------- */
    private static T Resolve<T>() where T : notnull
        => MauiProgram.AppServices.GetRequiredService<T>();
}
