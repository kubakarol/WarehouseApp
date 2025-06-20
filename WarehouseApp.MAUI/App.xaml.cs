using Microsoft.Maui.Storage;


namespace WarehouseApp.MAUI;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();

        var role = Preferences.Get("Role", "");
        if (string.IsNullOrEmpty(role))
        {
            Shell.Current.GoToAsync("//LoginPage");
        }
        else if (role == "Employee")
        {
            Shell.Current.GoToAsync("//InventoryPage");
        }
        else
        {
            Shell.Current.GoToAsync("//ShopPage");
        }
    }
}
