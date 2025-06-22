using CommunityToolkit.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WarehouseApp.MAUI.Pages;
using WarehouseApp.MAUI.Services;
using WarehouseApp.MAUI.ViewModels;

namespace WarehouseApp.MAUI;

public static class MauiProgram
{
    public static IServiceProvider AppServices { get; private set; } = default!;

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        /* ─────────── Podstawowa konfiguracja ─────────── */
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton(_ => new HttpClient
        {
            BaseAddress = new Uri("https://testwarehouse.azurewebsites.net") // bez /api na końcu
        });

        // Serwisy
        builder.Services.AddSingleton<AuthService>();
        builder.Services.AddSingleton<ItemService>();
        builder.Services.AddSingleton<INotificationService, NotificationService>();

        // ViewModel-e
        builder.Services.AddSingleton<InventoryViewModel>();
        builder.Services.AddTransient<AddItemViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();

        // Strony
        builder.Services.AddSingleton<Inventory>();       // magazyn
        builder.Services.AddSingleton<ShopPage>();        // sklep
        builder.Services.AddTransient<Pages.AddItemPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();

        /* ─────────── Budowanie aplikacji ─────────── */
        var app = builder.Build();

        // zapamiętujemy DI dla całej aplikacji
        AppServices = app.Services;

        return app;
    }
}
