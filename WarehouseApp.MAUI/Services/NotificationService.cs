using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Devices;

namespace WarehouseApp.MAUI.Services
{
    public interface INotificationService
    {
        Task SuccessAsync(string msg);
        Task ErrorAsync(string msg);
    }

    public class NotificationService : INotificationService
    {
        private static readonly ToastDuration Short = ToastDuration.Short;
        private static readonly ToastDuration Long = ToastDuration.Long;

        public NotificationService() { }

        public async Task SuccessAsync(string msg)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Toast.Make(msg, Short, 14).Show();
                TryVibrate(50);
            });
        }

        public async Task ErrorAsync(string msg)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Toast.Make(msg, Long, 14).Show();
                TryVibrate(300);
            });
        }

        private static void TryVibrate(int ms)
        {
#if ANDROID || IOS
            try { Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(ms)); }
            catch { }
#endif
        }
    }
}
