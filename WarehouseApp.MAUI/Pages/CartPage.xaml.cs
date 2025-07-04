﻿using WarehouseApp.Core;
using WarehouseApp.MAUI.Services;
using WarehouseApp.MAUI.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;
using System.Collections.ObjectModel;

namespace WarehouseApp.MAUI.Pages;

public partial class CartPage : ContentPage
{
    private readonly ObservableCollection<CartItemViewModel> _cart;
    private readonly ItemService _service = new(new HttpClient
    {
        BaseAddress = new Uri("https://warehouseapp-ebspazdhhbhgtb0e.westeurope-01.azurewebsites.net/api/Item")
    });

    public CartPage()
    {
        InitializeComponent();
        _cart = new ObservableCollection<CartItemViewModel>(
            CartStore.Cart.Select(c => new CartItemViewModel(c.Item, c.Count))
        );
        CartView.ItemsSource = _cart;
    }

    private void OnStepperValueChanged(object sender, ValueChangedEventArgs e)
    {
        if (sender is Stepper stepper && stepper.BindingContext is CartItemViewModel vm)
        {
            vm.Count = (int)e.NewValue;
        }
    }

    private async void OnRemoveClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is CartItemViewModel vm)
        {
            vm.Item.Quantity += vm.Count;
            _cart.Remove(vm);
            CartStore.Cart.RemoveAll(c => c.Item.Id == vm.Item.Id);
            await DisplayAlert("Usunięto", $"Usunięto {vm.Item.Name}", "OK");
        }
    }

    private async void OnPurchaseClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Potwierdzenie", "Na pewno chcesz zakupić produkty?", "Tak", "Nie");
        if (!confirm) return;

        foreach (var vm in _cart)
        {
            if (vm.Count > vm.Item.Quantity)
            {
                await DisplayAlert("Błąd", $"Nie ma wystarczającej ilości produktu: {vm.Item.Name}", "OK");
                return;
            }
        }

        foreach (var vm in _cart)
        {
            vm.Item.Quantity -= vm.Count;
            await _service.UpdateItemAsync(vm.Item);
        }

        try { Vibration.Default.Vibrate(300); } catch { }

        CartStore.Clear();
        _cart.Clear();

        await DisplayAlert("Sukces", "Zakup zakończony", "OK");
        await Shell.Current.GoToAsync("//ShopPage");
    }


    private async void OnBackToShopClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ShopPage");
    }
}
