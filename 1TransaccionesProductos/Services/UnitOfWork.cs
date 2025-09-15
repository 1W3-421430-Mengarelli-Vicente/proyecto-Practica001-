using _1TransaccionesProductos.Data;
using _1TransaccionesProductos.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1TransaccionesProductos.Services
{
    public class UnitOfWork
    {
        private InvoiceRepository invoiceRepo;
        private Invoice_DetailsRepository invoiceDetailsRepo;
        DataHelper dataHelper;

        public UnitOfWork()
        {
            invoiceRepo = new InvoiceRepository();
            invoiceDetailsRepo = new Invoice_DetailsRepository();
            dataHelper = DataHelper.GetInstance();
        }

        public bool SaveInvoiceWithDetails(Invoice i)
        {
            try
            {
                dataHelper.BeginTransaction();

                if (!invoiceRepo.Save(i)) { dataHelper.RollBack(); return false; }

                if (i.ListDetails != null && i.ListDetails.Count > 0)
                {
                    foreach (Invoice_Details detail in i.ListDetails)
                    {
                        detail.Invoice_Number = i.Number;
                    }
                    if (invoiceDetailsRepo.Save(i.ListDetails))
                    {
                        dataHelper.Commit();
                        return true;
                    }
                    else
                    {
                        dataHelper.RollBack();
                        return false;
                    }
                }
                else
                {
                    dataHelper.Commit();
                    return true;
                }
            }
            catch (Exception)
            {
                dataHelper.RollBack();
                throw;
            }
            finally
            {
                dataHelper.Cleanup();
            }
        }

        public List<Invoice>? BringAllInvoices()
        {
            return invoiceRepo.GetAll();
        }

        public bool DeleteInvoiceWithDetails(Invoice i)
        {
            return invoiceRepo.Delete(i.Number);
        }
    }
}
