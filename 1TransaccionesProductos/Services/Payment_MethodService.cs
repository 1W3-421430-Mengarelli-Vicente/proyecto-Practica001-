using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1TransaccionesProductos.Data;
using _1TransaccionesProductos.Domain;

namespace _1TransaccionesProductos.Services 
{
    public class Payment_MethodService
    {

        private IRepository<Payment_Method, int> PaymentMethodRepo;

        public Payment_MethodService()
        {
            PaymentMethodRepo = new Payment_MethodRepository();
        }

        public List<Payment_Method>? BringPaymentMethods()
        {
            return PaymentMethodRepo.GetAll();
        }
    }
}
