using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseApp.MAUI.ViewModels;

namespace WarehouseApp.MAUI.Pages;

public partial class InventoryPage : ContentPage
{
    public InventoryPage()
    {
        InitializeComponent();
        BindingContext = new ViewModels.InventoryViewModel();
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
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is InventoryViewModel vm)
        {
            await vm.LoadItemsAsync();
        }
    }


}
