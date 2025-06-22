using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using System.Net.Http.Headers;
using WarehouseApp.Core;
using WarehouseApp.MAUI.Services;

namespace WarehouseApp.MAUI.ViewModels;

public partial class AddItemViewModel : ObservableObject
{
    [ObservableProperty] private string name = string.Empty;
    [ObservableProperty] private string description = string.Empty;
    [ObservableProperty] private string quantity = string.Empty;
    [ObservableProperty] private ImageSource? image;

    private string? _imagePath;

    public IAsyncRelayCommand TakePhotoCommand { get; }
    public IAsyncRelayCommand PickPhotoCommand { get; }
    public IAsyncRelayCommand SaveCommand { get; }

    private readonly ItemService _items;
    private readonly INotificationService _toast;

    public AddItemViewModel(ItemService items, INotificationService toast)
    {
        _items = items;
        _toast = toast;

        TakePhotoCommand = new AsyncRelayCommand(TakePhotoAsync);
        PickPhotoCommand = new AsyncRelayCommand(PickPhotoAsync);
        SaveCommand = new AsyncRelayCommand(SaveAsync);
    }

    // !!! BEZ BaseAddress !!!
    public AddItemViewModel()
        : this(new ItemService(new HttpClient()), new NotificationService())
    { }

    private async Task TakePhotoAsync()
    {
        try
        {
#if ANDROID || IOS
            var status = await Permissions.RequestAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
            {
                await _toast.ErrorAsync("Brak uprawnień do aparatu");
                return;
            }

            var result = await MediaPicker.CapturePhotoAsync();

            if (result != null)
            {
                _imagePath = result.FullPath;
                Image = ImageSource.FromFile(_imagePath);
            }
#else
            await _toast.ErrorAsync("Robienie zdjęć działa tylko na urządzeniu mobilnym.");
#endif
        }
        catch (Exception ex)
        {
            await _toast.ErrorAsync("Nie udało się zrobić zdjęcia: " + ex.Message);
        }
    }

    private async Task PickPhotoAsync()
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Wybierz zdjęcie",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                _imagePath = result.FullPath;
                Image = ImageSource.FromFile(_imagePath);
            }
        }
        catch (Exception ex)
        {
            await _toast.ErrorAsync("Błąd przy wyborze zdjęcia: " + ex.Message);
        }
    }

    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(Name) ||
            string.IsNullOrWhiteSpace(Description) ||
            string.IsNullOrWhiteSpace(Quantity))
        {
            await _toast.ErrorAsync("Uzupełnij wszystkie pola");
            return;
        }

        if (!int.TryParse(Quantity, out _))
        {
            await _toast.ErrorAsync("Ilość musi być liczbą");
            return;
        }

        using var content = new MultipartFormDataContent
        {
            { new StringContent(Name),        "Name" },
            { new StringContent(Description), "Description" },
            { new StringContent(Quantity),    "Quantity" }
        };

        if (!string.IsNullOrWhiteSpace(_imagePath) && File.Exists(_imagePath))
        {
            var img = new StreamContent(File.OpenRead(_imagePath));
            img.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            content.Add(img, "Image", Path.GetFileName(_imagePath));
        }

        try
        {
            var ok = await _items.AddAsync(content).ConfigureAwait(false);
            if (!ok) throw new Exception("Serwer zwrócił błąd");

            await _toast.SuccessAsync("Dodano produkt");
            try { Vibration.Default.Vibrate(100); } catch { }

            ResetFields();

            await MainThread.InvokeOnMainThreadAsync(() =>
                Shell.Current.GoToAsync("//InventoryPage"));
        }
        catch (Exception ex)
        {
            await _toast.ErrorAsync($"Błąd: {ex.Message}");

            try { Vibration.Default.Vibrate(600); } catch { }

            System.Diagnostics.Debug.WriteLine(ex);
        }
    }

    private void ResetFields()
    {
        Name = string.Empty;
        Description = string.Empty;
        Quantity = string.Empty;
        Image = null;
        _imagePath = null;
    }
}
