using Library.eCommerce.Models;
using Library.eCommerce.Services;
using Spring2025_Samples.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Maui.eCommerce.ViewModels
{
    public class InventoryManagementViewModel : INotifyPropertyChanged
    {
        public Item? SelectedProduct { get; set; }
        public string? Query { get; set; }
        private ProductServiceProxy _svc = ProductServiceProxy.Current;
        private int _sortOption = 0; // 0 = Name, 1 = Price, 2 = Quantity

        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int SortOption
        {
            get => _sortOption;
            set
            {
                if (_sortOption != value)
                {
                    _sortOption = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(Products)); // Refresh products with new sort
                }
            }
        }

        public void RefreshProductList()
        {
            NotifyPropertyChanged(nameof(Products));
        }

        public ObservableCollection<Item?> Products
        {
            get
            {
                // Filter products by search query
                var filteredList = _svc.Products.Where(p => 
                    string.IsNullOrEmpty(Query) || 
                    (p?.Product?.Name?.ToLower().Contains(Query?.ToLower() ?? string.Empty) ?? false));

                // Apply sorting based on selected option
                IOrderedEnumerable<Item?> sortedList;
                
                switch (_sortOption)
                {
                    case 0: // Sort by Name
                        sortedList = filteredList.OrderBy(p => p?.Product?.Name);
                        break;
                    case 1: // Sort by Price
                        sortedList = filteredList.OrderBy(p => p?.Product?.Price);
                        break;
                    case 2: // Sort by Quantity
                        sortedList = filteredList.OrderBy(p => p?.Quantity);
                        break;
                    default:
                        sortedList = filteredList.OrderBy(p => p?.Product?.Name);
                        break;
                }

                return new ObservableCollection<Item?>(sortedList);
            }
        }

        public Item? Delete()
        {
            if (SelectedProduct == null) return null;
    
            var item = _svc.Delete(SelectedProduct.Id);
            NotifyPropertyChanged(nameof(Products));
            return item;
        }
    }
}