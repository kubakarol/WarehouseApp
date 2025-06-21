using System.Windows.Input;
using WarehouseApp.MAUI.Services;
using WarehouseApp.Core;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WarehouseApp.MAUI.ViewModels;

public class RegisterViewModel : INotifyPropertyChanged
{
    private readonly AuthService _authService;

    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string SelectedRole { get; set; } = "Client";

    public ICommand RegisterCommand { get; }
    public ICommand GoToLoginCommand { get; }

    public RegisterViewModel(AuthService authService)
    {
        _authService = authService;
        RegisterCommand = new Command(async () => await Register());
        GoToLoginCommand = new Command(async () => await Shell.Current.GoToAsync("//LoginPage"));
    }

    private async Task Register()
    {
        var user = new User
        {
            Username = Username,
            Password = Password,
            Role = SelectedRole
        };

        try 
        {
            Console.WriteLine("[DOTNET] [🔁] Sending register request to auth/register");

            var success = await _authService.RegisterAsync(user);
            if (success)
            {
                await Application.Current.MainPage.DisplayAlert("OK", "Rejestracja udana", "Zaloguj się");
                await Shell.Current.GoToAsync("//LoginPage");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Błąd", "Użytkownik już istnieje", "OK");
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"[DOTNET] [❌] HttpRequestException during registration: {ex.Message}");
            await Application.Current.MainPage.DisplayAlert("Błąd połączenia", "Nie można połączyć się z serwerem", "OK");
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
