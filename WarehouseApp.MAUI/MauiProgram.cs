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
            .UseMauiCommunityToolkit()      
            .ConfigureFonts(f =>
            {
                f.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                f.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton(_ => new HttpClient
        {
            BaseAddress = new Uri("https://testwarehouse.azurewebsites.net")
        });

        /* ---------- DI ---------- */
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddTransient<RegisterPage>(); 
        builder.Services.AddSingleton<AuthService>();
        builder.Services.AddSingleton<ItemService>();
        builder.Services.AddSingleton<INotificationService, NotificationService>();
        builder.Services.AddTransient<AddItemViewModel>();
        builder.Services.AddTransient<Pages.AddItemPage>();
        builder.Services.AddTransient<InventoryViewModel>();
        builder.Services.AddTransient<Inventory>();

        return builder.Build();
    }
}
 