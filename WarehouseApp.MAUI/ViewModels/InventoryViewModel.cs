using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WarehouseApp.Core;
using WarehouseApp.MAUI.Services;

namespace WarehouseApp.MAUI.ViewModels
{
    public class InventoryViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Item> Items { get; set; } = new();
        private readonly ItemService _itemService;

        public InventoryViewModel()
        {
            _itemService = new ItemService(new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7073")
            });
            LoadItemsAsync();
        }

        public async Task LoadItemsAsync()
        {
            var items = await _itemService.GetItemsAsync();
            Items.Clear();
            foreach (var item in items)
                Items.Add(item);
        }

        public void RefreshItem(Item item)
        {
            var index = Items.IndexOf(item);
            if (index >= 0)
            {
                Items.RemoveAt(index);
                Items.Insert(index, item);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
