using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Models.AccountModels
{
    public class ClienteAccountModel
    {
        //Propiedades.
        public string Cliente { get; set; }
        public string DisplayClienteNombre { get; set; }
        public byte[] ProfilePicture { get; set; }
    }
}