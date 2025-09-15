using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1TransaccionesProductos.Data;
using _1TransaccionesProductos.Domain;

namespace _1TransaccionesProductos.Services
{
    public class ProductService
    {
        private IRepository<Product, string> ProductRepo;

        public ProductService()
        {
            ProductRepo = new ProductRepository();
        }

        public List<Product>? GetAllProducts()
        {
            return ProductRepo.GetAll();
        }

        public Product? GetProductByCode(string code)
        {
            return ProductRepo.GetById(code);
        }

        private bool ValidateExisting(string code)
        {
            Product? b = ProductRepo.GetById(code);
            if (b == null) { return true; }
            else { return false; }
        }

        public bool InsertProduct(Product product)
        {
            if (ValidateExisting(product.Code))
            {
                return ProductRepo.Save(product);
            }
            else
            {
                Console.Write("The Code/Product in the DataBase already Exists\n");
                return false;
            }
        }

        public bool UpdateProduct(Product product)
        {
            if (!ValidateExisting(product.Code))
            {
                return ProductRepo.Save(product);
            }
            else
            {
                Console.Write("There is not a Product with that Code\n");
                return false;
            }
        }

        public bool UnsubscribeProduct(Product b)
        {
            if (!ValidateExisting(b.Code))
            {
                return ProductRepo.Delete(b.Code);
            }
            else
            {
                Console.Write("There is not a Product with that Code\n");
                return false;
            }
        }
    }
}
