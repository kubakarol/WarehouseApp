using WarehouseApp.MAUI.ViewModels;
using WarehouseApp.Core;

namespace WarehouseApp.MAUI.Pages;

public partial class ShopPage : ContentPage
{
    private readonly InventoryViewModel _viewModel;

    // Koszyk
    private readonly List<Item> _cart = new();

    public ShopPage()
    {
        InitializeComponent();
        _viewModel = new InventoryViewModel();
        BindingContext = _viewModel;
    }

    private async void OnAddToCartClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Item item)
        {
            if (item.Quantity > 0)
            {
                item.Quantity--;
                _cart.Add(item);
                _viewModel.RefreshItem(item);

                await DisplayAlert("Koszyk", $"Dodano: {item.Name}", "OK");
            }
            else
            {
                await DisplayAlert("Brak towaru", "Nie ma więcej sztuk tego produktu.", "OK");
            }
        }
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        Preferences.Set("Role", string.Empty);
        await Shell.Current.GoToAsync("//LoginPage");
    }

    private async void OnCartClicked(object sender, EventArgs e)
    {
        var count = _cart.Count;
        await DisplayAlert("Koszyk", $"Masz {count} produktów w koszyku.", "OK");
    }

}
