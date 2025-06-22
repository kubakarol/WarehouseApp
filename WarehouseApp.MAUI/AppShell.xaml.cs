using Microsoft.Maui.Controls;
using WarehouseApp.MAUI.Pages;

namespace WarehouseApp.MAUI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // 📌 rejestrujemy trasę AddItemPage => można używać nameof(AddItemPage)
        Routing.RegisterRoute(nameof(AddItemPage), typeof(AddItemPage));
    }
}
