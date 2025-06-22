using WarehouseApp.MAUI.ViewModels;
using WarehouseApp.Core;
using WarehouseApp.MAUI.Services;

namespace WarehouseApp.MAUI.Pages;

public partial class ShopPage : ContentPage
{
    private readonly InventoryViewModel _viewModel;

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
                var existing = CartStore.Cart.FirstOrDefault(i => i.Item.Id == item.Id);
                if (existing.Item != null)
                {
                    int index = CartStore.Cart.FindIndex(i => i.Item.Id == item.Id);
                    CartStore.Cart[index] = (CartStore.Cart[index].Item, CartStore.Cart[index].Count + 1);
                }
                else
                {
                    CartStore.Cart.Add((item, 1));
                }

                _viewModel.RefreshItem(item);

                try { Vibration.Default.Vibrate(100); } catch { }

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
        await Shell.Current.Navigation.PushAsync(new CartPage());
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadItemsAsync();
    }
}
