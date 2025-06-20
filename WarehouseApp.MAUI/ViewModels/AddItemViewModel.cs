using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using WarehouseApp.MAUI.Services;
using System.ComponentModel;
using System.Net.Http.Headers;

namespace WarehouseApp.MAUI.ViewModels
{
    public class AddItemViewModel : INotifyPropertyChanged
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Quantity { get; set; } = "";
        public ImageSource? Image { get; set; }

        private string? _imagePath;

        public ICommand TakePhotoCommand { get; }
        public ICommand SaveCommand { get; }

        private readonly ItemService _service = new(new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7073") // <- zamień na prawdziwy adres API
        });

        public AddItemViewModel()
        {
            TakePhotoCommand = new AsyncRelayCommand(TakePhotoAsync);
            SaveCommand = new AsyncRelayCommand(SaveAsync);
        }

        private async Task TakePhotoAsync()
        {
            var result = await MediaPicker.CapturePhotoAsync();
            if (result != null)
            {
                _imagePath = result.FullPath;
                Image = ImageSource.FromFile(_imagePath);
                OnPropertyChanged(nameof(Image));
            }
        }

        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(Description) ||
                string.IsNullOrWhiteSpace(Quantity))
            {
                await Shell.Current.DisplayAlert("Błąd", "Uzupełnij wszystkie pola", "OK");
                return;
            }

            if (!int.TryParse(Quantity, out int qty))
            {
                await Shell.Current.DisplayAlert("Błąd", "Niepoprawna liczba w polu ilość", "OK");
                return;
            }

            using var content = new MultipartFormDataContent();

            content.Add(new StringContent(Name), "Name");
            content.Add(new StringContent(Description), "Description");
            content.Add(new StringContent(Quantity), "Quantity");

            if (!string.IsNullOrWhiteSpace(_imagePath) && File.Exists(_imagePath))
            {
                var imageContent = new StreamContent(File.OpenRead(_imagePath));
                imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                content.Add(imageContent, "Image", Path.GetFileName(_imagePath));
            }

            var success = await _service.AddItemAsync(content);

            if (success)
            {
                Name = "";
                Description = "";
                Quantity = "";
                Image = null;
                _imagePath = null;

                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(Quantity));
                OnPropertyChanged(nameof(Image));

                await Shell.Current.GoToAsync("//InventoryPage");
            }
            else
            {
                await Shell.Current.DisplayAlert("Błąd", "Nie udało się zapisać produktu", "OK");
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
