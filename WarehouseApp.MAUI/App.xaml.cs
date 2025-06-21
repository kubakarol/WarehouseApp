using Microsoft.Maui.Storage;


namespace WarehouseApp.MAUI;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }

    protected override void OnStart()
    {
        base.OnStart();

        var role = Preferences.Get("Role", "");
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            if (string.IsNullOrEmpty(role))
                await Shell.Current.GoToAsync("//LoginPage");
            else if (role == "Employee")
                await Shell.Current.GoToAsync("//InventoryPage");
            else
                await Shell.Current.GoToAsync("//ShopPage");
        });
    }
}

