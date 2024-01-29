using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.HanlderExceptions
{
    public class VentasExceptions : Exception
    {
        //Propiedades.
        public VentasExceptions() : base() { }

        public VentasExceptions(string message) : base(message) { }

        public VentasExceptions(string message, Exception innerException) : base(message, innerException) { }
    }
}
