using _1TransaccionesProductos.Domain;
using _1TransaccionesProductos.Services;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class ProductServiceAPI : IProductService
    {
        private readonly ProductService _productService;

        public ProductServiceAPI()
        {
            _productService = new ProductService();
        }
        public List<Product>? GetAll()
        {
            return _productService.GetAllProducts();
        }
        public Product? GetById(string code)
        {
            return _productService.GetProductByCode(code);
        }
        public bool Create(Product product)
        {
            return _productService.InsertProduct(product);
        }
        public bool Update(Product product)
        {
            return _productService.UpdateProduct(product);
        }
        public bool Delete(string code)
        {
            var p = _productService.GetProductByCode(code);
            if (p == null) return false;
            return _productService.UnsubscribeProduct(p);
        }
    }
}
