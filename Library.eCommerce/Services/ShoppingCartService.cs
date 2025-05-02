using Library.eCommerce.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Library.eCommerce.Services
{
    public class ShoppingCartService
    {
        private static ShoppingCartService? _instance;
        private Dictionary<string, ObservableCollection<Item?>> _carts = new Dictionary<string, ObservableCollection<Item?>>();
        private string _currentCart = "Default";
        private ProductServiceProxy _productService;
        
        public static ShoppingCartService Current => _instance ??= new ShoppingCartService();

        public ObservableCollection<Item?> CartItems => _carts.ContainsKey(_currentCart) ? 
            _carts[_currentCart] : (_carts[_currentCart] = new ObservableCollection<Item?>());
        
        public List<string> CartNames => _carts.Keys.ToList();
        
        public string CurrentCart 
        { 
            get => _currentCart; 
            set 
            {
                if (_currentCart != value && !string.IsNullOrEmpty(value))
                {
                    _currentCart = value;
                    
                    // Ensure the cart exists
                    if (!_carts.ContainsKey(_currentCart))
                    {
                        _carts[_currentCart] = new ObservableCollection<Item?>();
                    }
                    
                    // Notify of change
                    CartChanged?.Invoke(this, EventArgs.Empty);
                }
            } 
        }
        
        // Add event to notify when cart changes
        public event EventHandler? CartChanged;

        private ShoppingCartService()
        {
            _productService = ProductServiceProxy.Current;
            _carts["Default"] = new ObservableCollection<Item?>();
        }
    }
}