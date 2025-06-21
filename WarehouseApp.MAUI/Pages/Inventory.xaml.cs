using Microsoft.Maui.Controls;
using WarehouseApp.MAUI.ViewModels;

namespace WarehouseApp.MAUI.Pages;

/// <summary>
/// Strona magazynowa – odświeża dane za każdym pojawieniem się.
/// </summary>
public partial class Inventory : ContentPage
{
    public Inventory(InventoryViewModel vm)
    {
        InitializeComponent();     // ← teraz kompilator wygeneruje tę metodę
        BindingContext = vm;

        // pierwsze pobranie listy
        _ = vm.LoadAsync();
    }

    /// <summary>Wywoływane zawsze, gdy użytkownik wraca do tej strony.</summary>
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is InventoryViewModel vm)
            await vm.LoadAsync();  // pobranie najnowszych danych z API
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        Preferences.Set("Role", string.Empty);
        await Shell.Current.GoToAsync("//LoginPage");
    }

    private async void OnAddItemClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//AddItemPage");
    }
}
