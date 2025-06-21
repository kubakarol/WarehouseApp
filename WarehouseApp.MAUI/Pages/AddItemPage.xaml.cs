using WarehouseApp.MAUI.ViewModels;

namespace WarehouseApp.MAUI.Pages;

public partial class AddItemPage : ContentPage
{
    public AddItemPage(AddItemViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
