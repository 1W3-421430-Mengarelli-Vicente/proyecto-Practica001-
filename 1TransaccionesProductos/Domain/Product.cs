using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1TransaccionesProductos.Domain 
{
    public class Product
    {
        public string Code { get; set; } // codigo
        public string Name { get; set; } // nombre
        public string Brand { get; set; } // marca
        public string Specs { get; set; } // especificaciones
        public double Price { get; set; }
        public int Stock { get; set; }

        public Product()
        {
            Code = string.Empty;
            Name = string.Empty;
            Brand = string.Empty;
            Specs = string.Empty;
            Price = 0;
            Stock = 0;
        }

        public Product(string code, string name, string brand, string specs, double price, int stock)
        {
            this.Code = code;
            this.Name = name;
            this.Brand = brand;
            this.Specs = specs;
            this.Price = price;
            this.Stock = stock;
        }
        public override string ToString()
        {
            return $"{Name} ({Brand}) - ${Price:F2} | Stock: {Stock}";
        }
    }
}
