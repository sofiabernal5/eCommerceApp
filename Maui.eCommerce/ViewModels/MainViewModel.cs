using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.eCommerce.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Library.eCommerce.Services;
using Spring2025_Samples.Models;

namespace Maui.eCommerce.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ProductServiceProxy _productService;
        private readonly ShoppingCartService _cartService;

        public ObservableCollection<Product> Products { get; } = new();
        public ObservableCollection<Item> CartItems { get; } = new();

        public ICommand LoadCommand { get; }
        public ICommand AddProductCommand { get; }
        public ICommand DeleteProductCommand { get; }
        public ICommand AddToCartCommand { get; }
        public ICommand RemoveFromCartCommand { get; }

        public string NewProductName { get; set; }
        public double NewProductPrice { get; set; }
        public int NewProductQuantity { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainViewModel()
        {
            _productService = new ProductServiceProxy();
            _cartService = new ShoppingCartService();

            LoadCommand = new Command(LoadData);
            AddProductCommand = new Command(AddProduct);
            DeleteProductCommand = new Command<int>(DeleteProduct);
            AddToCartCommand = new Command<int>(AddToCart);
            RemoveFromCartCommand = new Command<int>(RemoveFromCart);

            LoadData();
        }

        private void LoadData()
        {
            Products.Clear();
            foreach (var product in _productService.GetAll())
                Products.Add(product);

            CartItems.Clear();
            foreach (var item in _cartService.Cart)
                CartItems.Add(item);
        }

        private void AddProduct()
        {
            var newProduct = new Product
            {
                Name = NewProductName,
                Price = NewProductPrice,
                Quantity = NewProductQuantity
            };
            _productService.AddOrUpdate(newProduct);
            LoadData();
        }

        private void DeleteProduct(int id)
        {
            _productService.Delete(id);
            LoadData();
        }

        private void AddToCart(int productId)
        {
            var product = _productService.GetById(productId);
            if (product != null)
            {
                _cartService.AddToCart(product, 1);
                LoadData();
            }
        }

        private void RemoveFromCart(int productId)
        {
            _cartService.RemoveFromCart(productId);
            LoadData();
        }

        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}