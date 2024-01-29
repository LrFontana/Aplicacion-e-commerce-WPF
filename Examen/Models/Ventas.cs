using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Models
{
    public class Ventas
    {
        //Propiedades.
        [Key]
        public int ID { get; set; }

        public DateTime Fecha { get; set; }

        [Column(TypeName = "float")]
        public double Total { get; set; }

        public int IDCliente { get; set; }

        [ForeignKey("IDCliente")]
        public virtual Clientes Cliente { get; set; }
    }
}
