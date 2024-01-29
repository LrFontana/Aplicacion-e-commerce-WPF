using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.HanlderExceptions
{
    public class ClientesExceptions: Exception
    {
        //Propiedades.
        public ClientesExceptions() : base() { }

        public ClientesExceptions(string message) : base(message) { }

        public ClientesExceptions(string message, Exception innerException) : base(message, innerException) { }
    }
}
