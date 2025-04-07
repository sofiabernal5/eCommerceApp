using Library.eCommerce.Models;
using Library.eCommerce.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Maui.eCommerce.ViewModels
{
    public class ShoppingManagementViewModel : INotifyPropertyChanged
    {
        private ProductServiceProxy _invSvc;
        private ShoppingCartService _cartSvc;
        private ObservableCollection<Item?> _inventory;
        private ObservableCollection<Item> _shoppingCart;
        private Item? _selectedProduct;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ShoppingManagementViewModel()
        {
            _invSvc = ProductServiceProxy.Current;
            _cartSvc = ShoppingCartService.Current;
            _inventory = new ObservableCollection<Item?>(_invSvc.Products);
            _shoppingCart = new ObservableCollection<Item>(_cartSvc.CartItems);
        }

        public ObservableCollection<Item?> Inventory
        {
            get { return _inventory; }
        }

        public ObservableCollection<Item> ShoppingCart
        {
            get { return _cartSvc.CartItems; }
        }

        public Item? SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                NotifyPropertyChanged(nameof(SelectedProduct));
            }
        }

        public void AddToCart()
        {
            
            if (_selectedProduct != null)
            {
                Console.WriteLine($"Adding {SelectedProduct.Product.Name} to cart...");
                _cartSvc.AddToCart(_selectedProduct);
                NotifyPropertyChanged(nameof(ShoppingCart)); // Refresh UI
            }
        }

        public void RefreshInventory()
        {
            NotifyPropertyChanged(nameof(Inventory));
        }

        public void RefreshShoppingCart()
        {
            NotifyPropertyChanged(nameof(ShoppingCart));
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}