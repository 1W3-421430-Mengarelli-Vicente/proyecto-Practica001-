using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1TransaccionesProductos.Data
{
    internal class Parameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public bool isOutput { get; set; }

        public Parameter(string Name, object Value, bool isOutput = false)
        {
            this.Name = Name;
            this.Value = Value;
            this.isOutput = isOutput;
        }
    }
}
