using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Work done by Sofia Bernal
namespace Spring2025_Samples.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public double Price { get; set; }
        
        public int Quantity { get; set; }
        public string? Display
        {
            get
            {
                return $"{Id}. {Name}, Quantity: {Quantity}, Price: ${Price} ";
            }
        }

        public Product()
        {
            Name = string.Empty;
        }

        public override string ToString()
        {
            return Display ?? string.Empty;
        }
    }
}
