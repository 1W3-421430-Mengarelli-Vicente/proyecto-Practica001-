
using _1TransaccionesProductos.Domain;
using _1TransaccionesProductos.Services;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class FacturaService : IFacturaService
    {
        private readonly UnitOfWork _unitOfWork;
        public FacturaService() 
        {
            _unitOfWork = new UnitOfWork();
        }

        public bool CreateInvoice(Invoice invoice)
        {
            return _unitOfWork.SaveInvoiceWithDetails(invoice);
        }

        public bool DeleteInvoice(int invoiceNumber)
        {
            var factura = new Invoice() { Number = invoiceNumber };
            return _unitOfWork.DeleteInvoiceWithDetails(factura);
        }

        public List<Invoice> GetAllInvoices()
        {
            return _unitOfWork.BringAllInvoices();
        }
    }
}
