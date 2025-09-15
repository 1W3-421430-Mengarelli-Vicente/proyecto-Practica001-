using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1TransaccionesProductos.Domain;

namespace _1TransaccionesProductos.Data
{
    internal class Payment_MethodRepository : IRepository<Payment_Method, int>
    {
        public bool Delete(int entity)
        {
            throw new NotImplementedException();
        }

        public List<Payment_Method>? GetAll()
        {
            List<Payment_Method> paymentMethods = new List<Payment_Method>();

            foreach (DataRow row in DataHelper.GetInstance().ExecuteSPRead("SP_OBTENER_METODOS_PAGOS").Rows)
            {
                paymentMethods.Add(new Payment_Method()
                {
                    Id = Convert.ToInt32(row["id_forma_pago"]),
                    Description = (string)row["forma_pago"]
                });
            }

            return paymentMethods;
        }

        public Payment_Method? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Save(Payment_Method entity)
        {
            throw new NotImplementedException();
        }
    }
}
