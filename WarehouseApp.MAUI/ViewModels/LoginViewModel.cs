using System.Windows.Input;
using WarehouseApp.MAUI.Services;
using WarehouseApp.Core;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WarehouseApp.MAUI.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly AuthService _authService;

        public string Username { get; set; } = "";
        public string Password { get; set; } = "";

        public ICommand LoginCommand { get; }
        public ICommand GoToRegisterCommand { get; }

        public LoginViewModel()
        {
            _authService = new AuthService(new HttpClient());
            LoginCommand = new Command(async () => await Login());
            GoToRegisterCommand = new Command(async () => await Shell.Current.GoToAsync("//RegisterPage"));
        }

        private async Task Login()
        {
            var user = await _authService.LoginAsync(Username, Password);
            if (user is null)
            {
                await Application.Current.MainPage.DisplayAlert("Błąd", "Niepoprawne dane logowania", "OK");
                return;
            }

            // zapis danych użytkownika lokalnie
            Preferences.Set("UserId", user.Id);
            Preferences.Set("Username", user.Username);
            Preferences.Set("Role", user.Role);

            // przełącz na odpowiedni widok
            if (user.Role == "Employee")
                await Shell.Current.GoToAsync("//InventoryPage");
            else
                await Shell.Current.GoToAsync("//ShopPage");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
