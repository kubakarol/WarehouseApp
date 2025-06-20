namespace WarehouseApp.MAUI.Pages;

public partial class AddItemPage : ContentPage
{
    public AddItemPage()
    {
        InitializeComponent();
        BindingContext = new ViewModels.AddItemViewModel();
    }
}
