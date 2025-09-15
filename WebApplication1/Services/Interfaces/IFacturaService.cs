using _1TransaccionesProductos.Domain;

namespace WebApplication1.Services.Interfaces
{
    public interface IFacturaService
    {
        List<Invoice> GetAllInvoices();
        bool CreateInvoice(Invoice invoice);
        bool DeleteInvoice(int invoiceNumber);
    }
}
