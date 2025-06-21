using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using WarehouseApp.MAUI.Pages;
using WarehouseApp.MAUI.Services;
using WarehouseApp.MAUI.ViewModels;

namespace WarehouseApp.MAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()      // <-- WAŻNE
            .ConfigureFonts(f =>
            {
                f.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                f.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        /* ---------- HttpClient ---------- */
        builder.Services.AddSingleton(_ => new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7073/api/")
        });

        /* ---------- DI ---------- */
        builder.Services.AddSingleton<ItemService>();
        builder.Services.AddSingleton<INotificationService, NotificationService>();
        builder.Services.AddTransient<AddItemViewModel>();
        builder.Services.AddTransient<Pages.AddItemPage>();
        builder.Services.AddTransient<InventoryViewModel>();
        builder.Services.AddTransient<Inventory>();

        return builder.Build();
    }
}
