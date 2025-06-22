using Microsoft.Maui.Controls;
using WarehouseApp.MAUI.ViewModels;

namespace WarehouseApp.MAUI.Pages;

public partial class AddItemPage : ContentPage
{
    public AddItemPage(AddItemViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    // działa dla ToolbarItem "Cofnij"
    private async void OnBackClicked(object sender, EventArgs e)
        => await Shell.Current.GoToAsync("..");
}
