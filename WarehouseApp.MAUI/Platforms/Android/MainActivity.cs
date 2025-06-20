#if ANDROID
using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.LocalNotification;

namespace WarehouseApp.MAUI
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation |
        ConfigChanges.UiMode | ConfigChanges.ScreenLayout |
        ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
            {
                const string permission = Android.Manifest.Permission.PostNotifications;
                if (CheckSelfPermission(permission) != Permission.Granted)
                {
                    RequestPermissions(new[] { permission }, 0);
                }
            }
        }
    }
}
#endif
