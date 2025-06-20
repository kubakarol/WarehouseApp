using System.ComponentModel;
using WarehouseApp.Core;


namespace WarehouseApp.MAUI.ViewModels;

public class CartItemViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public Item Item { get; set; }

    private int _count;
    public int Count
    {
        get => _count;
        set
        {
            if (_count != value)
            {
                _count = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
            }
        }
    }

    public CartItemViewModel(Item item, int count)
    {
        Item = item;
        _count = count;
    }
}
