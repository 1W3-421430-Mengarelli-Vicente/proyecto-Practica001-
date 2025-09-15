using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1TransaccionesProductos.Domain;

namespace _1TransaccionesProductos.Data
{
    public class InvoiceRepository : IRepository<Invoice, int>
    {

        Invoice_DetailsRepository InvoiceDetailsRepo;

        public InvoiceRepository()
        {
            InvoiceDetailsRepo = new Invoice_DetailsRepository();
        }

        public bool Delete(int invoiceNumber)
        {
            List<Parameter> p = new List<Parameter>();
            p.Add(new Parameter("@nro_factura", invoiceNumber));

            return DataHelper.GetInstance().ExecuteSPModify("SP_ELIMINAR_FACTURA", p).affectedRows > 0;         
        }

        public List<Invoice>? GetAll()
        {
            List<Invoice> invoices = new List<Invoice>();
            
            foreach (DataRow row in DataHelper.GetInstance().ExecuteSPRead("SP_OBTENER_FACTURAS").Rows)
            {
                invoices.Add(new Invoice()
                {
                    Number = Convert.ToInt32(row["nro_factura"]),
                    Date = Convert.ToDateTime(row["fecha"]),
                    Client = (string)row["cliente"],
                    Payment_Method = new Payment_Method()
                    {
                        Id = Convert.ToInt32(row["id_forma_pago"]),
                        Description = (string)row["forma_pago"]
                    },
                    ListDetails = InvoiceDetailsRepo.GetAll(Convert.ToInt32(row["nro_factura"]))
                });
            }

            return invoices;
        }

        public Invoice? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Save(Invoice i)
        {
            List<Parameter> p = new List<Parameter>();
           
            p.Add(new Parameter("@nro_factura", i.Number));
            p.Add(new Parameter("@fecha", i.Date));
            p.Add(new Parameter("@id_forma_pago", i.Payment_Method.Id));
            p.Add(new Parameter("@cliente", i.Client));
            p.Add(new Parameter("@new_id", 0, true));

            var (affectedRows, newId) = DataHelper.GetInstance().ExecuteSPModify("SP_MODIFICAR_FACTURAS", p);

            if (newId > 0)
            {
                i.Number = newId;
            }

            return affectedRows > 0;
        }

        public decimal GetTotal(int nroFactura)
        {
            Parameter p = new Parameter("@nro_factura", nroFactura);
            return DataHelper.GetInstance().ExecuteSPScalarDecimal("SP_OBTENER_TOTAL_FACTURA", p);
        }

        public int GetLastInvoiceNumber()
        {
            DataTable dt = DataHelper.GetInstance().ExecuteSPRead("SP_OBTENER_ULTIMA_FACTURA");
            if (dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["nro_factura"]);
            }
            return 0;
        }

    }
}
