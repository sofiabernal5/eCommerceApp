using Library.eCommerce.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Library.eCommerce.Services
{
    public class ShoppingCartService
    {
        private static ShoppingCartService? _instance;
        private ObservableCollection<Item?> _items = new ObservableCollection<Item?>(); 
        public static ShoppingCartService Current => _instance ??= new ShoppingCartService();

        public ObservableCollection<Item?> CartItems => _items;

        public void AddToCart(Item item)
        {
            if (item != null)
                _items.Add(item);
        }

        public void RemoveFromCart(Item item)
        {
            if (_items.Contains(item))
                _items.Remove(item);
        }
    }
}