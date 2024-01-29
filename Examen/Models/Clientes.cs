using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Models
{
    public class Clientes
    {
        //Propiedades.
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Cliente { get; set; }

        [StringLength(20)]
        public string Telefono { get; set; }

        [EmailAddress]
        [StringLength(255)]
        public string Correo { get; set; }
    }
}
