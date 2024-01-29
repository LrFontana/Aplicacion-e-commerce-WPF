using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.HanlderExceptions
{
    public class ProductosExceptions: Exception
    {
        //Propiedades.
        public ProductosExceptions() : base() { }

        public ProductosExceptions(string message) : base(message) { }

        public ProductosExceptions(string message, Exception innerException) : base(message, innerException) { }
    }
}
