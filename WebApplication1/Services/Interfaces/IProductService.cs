using _1TransaccionesProductos.Domain;

namespace WebApplication1.Services.Interfaces
{
    public interface IProductService
    {
        List<Product>? GetAll();
        Product? GetById(string code);
        bool Create(Product product);
        bool Update(Product product);
        bool Delete(string code);

    }
}
