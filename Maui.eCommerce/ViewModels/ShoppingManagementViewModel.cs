using Library.eCommerce.Models;
using Library.eCommerce.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.eCommerce.ViewModels
{
    public class ShoppingManagementViewModel
    {
        public ObservableCollection<Item> Inventory { get; set; }
        public ObservableCollection<Item> CartItems { get; set; }
        public Item? SelectedItem { get; set; }

        public ShoppingManagementViewModel()
        {
            Inventory = new ObservableCollection<Item>(
                ProductServiceProxy.Current.Products.Select(p => new Item
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Product = p,
                    Quantity = 1
                }).ToList()
            );
            CartItems = new ObservableCollection<Item>(ShoppingCartService.Current.CartItems);
        }

        public void AddToCart()
        {
            if (SelectedItem != null)
            {
                ShoppingCartService.Current.AddItem(SelectedItem);
                RefreshCart();
            }
        }

        public void RefreshCart()
        {
            CartItems.Clear();
            foreach (var item in ShoppingCartService.Current.CartItems)
            {
                CartItems.Add(item);
            }
        }
    }
}