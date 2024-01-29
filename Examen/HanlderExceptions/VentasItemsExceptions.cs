using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.HanlderExceptions
{
    public class VentasItemsExceptions: Exception
    {
        //Propiedades.
        public VentasItemsExceptions() : base() { }

        public VentasItemsExceptions(string message) : base(message) { }

        public VentasItemsExceptions(string message, Exception innerException) : base(message, innerException) { }
    }
}
