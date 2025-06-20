// Services/CartStore.cs
using WarehouseApp.Core;

namespace WarehouseApp.MAUI.Services;

public static class CartStore
{
    public static List<(Item Item, int Count)> Cart { get; set; } = new();

    public static void Clear() => Cart.Clear();
}
