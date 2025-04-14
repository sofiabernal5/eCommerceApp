using Library.eCommerce.Models;
using Library.eCommerce.Services;
using Spring2025_Samples.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Maui.eCommerce.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private Item _model;
        
        public event PropertyChangedEventHandler? PropertyChanged;
        
        public ProductViewModel()
        {
            _model = new Item
            {
                Product = new Product(),
                Quantity = 0,
                Price = 0
            };
        }

        public ProductViewModel(Item? model)
        {
            _model = model ?? new Item
            {
                Product = new Product(),
                Quantity = 0,
                Price = 0
            };
        }
        
        public string? Name
        { 
            get => _model?.Product?.Name ?? string.Empty;
            set
            {
                if(_model?.Product != null && _model.Product.Name != value)
                {
                    _model.Product.Name = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public int Quantity
        {
            get => _model?.Quantity ?? 0;
            set
            {
                if(_model != null && _model.Quantity != value)
                {
                    _model.Quantity = value;
                    NotifyPropertyChanged();
                }
            }
        }
        
        public double Price
        {
            get => _model?.Product?.Price ?? 0;
            set
            {
                if(_model?.Product != null && _model.Product.Price != value)
                {
                    _model.Product.Price = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Item Model => _model;

        public void AddOrUpdate()
        {
            if (_model != null)
            {
                ProductServiceProxy.Current.AddOrUpdate(_model);
            }
        }
        
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}