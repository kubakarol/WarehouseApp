using Microsoft.Extensions.Logging;
using WarehouseApp.MAUI.ViewModels;
using WarehouseApp.MAUI.Pages;

namespace WarehouseApp.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Rejestracja tylko ViewModel i strony magazynowej
            builder.Services.AddSingleton<InventoryViewModel>();
            builder.Services.AddSingleton<InventoryPage>();

            return builder.Build();
        }
    }
}
