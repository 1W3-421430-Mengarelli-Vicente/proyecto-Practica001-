using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1TransaccionesProductos.Domain   
{
    public class Invoice_Details
    {
        public Product Product { get; set; }
        public int Amount { get; set; }
        public int Invoice_Number { get; set; }
        public double SubTotal => (double)Product.Price * Amount;


        public Invoice_Details()
        {
            Product = new Product();
            Amount = 0;
            Invoice_Number = 0;
        }

        public Invoice_Details(Product product, int Amount, int Invoice_Number)
        {
            this.Product = product;
            this.Amount = Amount;
            this.Invoice_Number = Invoice_Number;
        }

        public override string ToString()
        {
            return $"{Product.Name} x {Amount} = ${SubTotal:F2}";
        }
    }
}
