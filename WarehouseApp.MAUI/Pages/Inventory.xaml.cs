using Microsoft.Maui.Controls;
using WarehouseApp.MAUI.ViewModels;

namespace WarehouseApp.MAUI.Pages;

public partial class Inventory : ContentPage
{
    public Inventory(InventoryViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        _ = vm.LoadAsync();          // pierwsze ładowanie
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is InventoryViewModel vm)
            await vm.LoadAsync();    // zawsze świeże dane
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
        => await Shell.Current.GoToAsync("//LoginPage");

    private async void OnAddItemClicked(object sender, EventArgs e)
        // ⚠️ NAZWA trasy = nameof(AddItemPage)  (bez //)
        => await Shell.Current.GoToAsync(nameof(AddItemPage));
}
