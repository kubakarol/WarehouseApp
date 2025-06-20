using WarehouseApp.MAUI.ViewModels;
using WarehouseApp.Core;

namespace WarehouseApp.MAUI.Pages;

public partial class ShopPage : ContentPage
{
    private readonly InventoryViewModel _viewModel;
    private readonly List<(Item item, int count)> _cart = new();

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
                var existing = _cart.FirstOrDefault(i => i.item.Id == item.Id);
                if (_cart.Any(i => i.item.Id == item.Id))
                {
                    int index = _cart.FindIndex(i => i.item.Id == item.Id);
                    _cart[index] = (_cart[index].item, _cart[index].count + 1);
                }
                else
                {
                    _cart.Add((item, 1));
                }

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
        await Shell.Current.Navigation.PushAsync(new CartPage(_cart));
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadItemsAsync();
    }

}
