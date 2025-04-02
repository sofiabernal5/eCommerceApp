using Spring2025_Samples.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.eCommerce.Models
{
    public class Item
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        
        public double Price { get; set; }
        public string? Name { get; set; }
        public int? Quantity { get; set; }

        public override string ToString()
        {
            return $"{Product} Quantity:{Quantity}";
        }

        public string Display
        {
            get
            {
                var price = Product?.Price ?? 0;
                return $"{Name}, Quantity: {Quantity}, Price: ${price * Quantity:F2}";
            }
        }

        public Item()
        {
            Product = new Product();
        }
    }
}