using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1TransaccionesProductos.Domain;

namespace _1TransaccionesProductos.Data
{
    internal class ProductRepository : IRepository<Product, string>
    {
        public bool Delete(string code)
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("@codigo", code));

            var (affectedRows, newId) = DataHelper.GetInstance().ExecuteSPModify("SP_DAR_BAJA_PRODUCTO", parameters);

            return affectedRows > 0;
        }

        public List<Product>? GetAll()
        {
            List<Product> products = new List<Product>();
            DataTable dt = new DataTable();

            dt = DataHelper.GetInstance().ExecuteSPRead("SP_OBTENER_PRODUCTOS");
            
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    products.Add(new Product()
                    {
                        Code = (string)row["codigo"],
                        Name = (string)row["nombre"],
                        Brand = (string)row["marca"],
                        Specs = (string)(row["especificaciones"]),
                        Price = Convert.ToDouble(row["precio_unitario"]),
                        Stock = Convert.ToInt32(row["stock"])
                    });
                }

                return products;
            }
            else
            {
                return null;
            }
        }

        public Product? GetById(string code)
        {
            DataTable dt = new DataTable();
            Parameter? p;

            if (code != null) { p = new Parameter("@codigo", code); }
            else { p = null; }

            dt = DataHelper.GetInstance().ExecuteSPRead("SP_OBTENER_PRODUCTO_POR_CODIGO", p);          

            if (dt.Rows.Count > 0)
            {
                Product product = new Product()
                {
                    Code = (string)dt.Rows[0]["codigo"],
                    Name = (string)dt.Rows[0]["nombre"],
                    Brand = (string)dt.Rows[0]["marca"],
                    Specs = (string)(dt.Rows[0]["especificaciones"]),
                    Price = Convert.ToDouble(dt.Rows[0]["precio_unitario"]),
                    Stock = Convert.ToInt32(dt.Rows[0]["stock"])
                };
                return product;
            }
            else
            {
                return null;
            }
        }

        public bool Save(Product product)
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("@codigo", product.Code));
            parameters.Add(new Parameter("@nombre", product.Name));
            parameters.Add(new Parameter("@marca", product.Brand));
            parameters.Add(new Parameter("@especificaciones", product.Specs));
            parameters.Add(new Parameter("@precio_unitario", product.Price));
            parameters.Add(new Parameter("@stock", product.Stock));

            var (affectedRows, newId) = DataHelper.GetInstance().ExecuteSPModify("SP_MODIFICAR_PRODUCTOS", parameters);

            return affectedRows > 0;          
        }
    }
}
