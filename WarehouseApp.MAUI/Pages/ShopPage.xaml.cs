using WarehouseApp.MAUI.ViewModels;
using WarehouseApp.Core;

namespace WarehouseApp.MAUI.Pages;

public partial class ShopPage : ContentPage
{
    public ShopPage()
    {
        InitializeComponent();
        BindingContext = new InventoryViewModel();
    }

    private async void OnAddToCartClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Item item)
        {
            await DisplayAlert("Koszyk", $"Dodano: {item.Name}", "OK");
        }
    }
}
