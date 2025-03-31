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
        private ProductServiceProxy _invSvc = ProductServiceProxy.Current;

        public ObservableCollection<Item?> Inventory
        {
            get
            {
                var itemList = _invSvc.Products.Select(p => new Item
                {
                    Product = p,
                    Name = p.Name,
                    Quantity = 0  // Set the quantity as needed
                }).ToList();

                return new ObservableCollection<Item?>(itemList);
            }
        }

    }
}