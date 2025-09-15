using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1TransaccionesProductos.Data
{
    internal interface IRepository<T,D>
    {
        List<T>? GetAll();
        T? GetById(D id);
        bool Save(T entity);
        bool Delete(D entity);
    }
}
