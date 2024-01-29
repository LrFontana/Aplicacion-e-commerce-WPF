using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Models
{
    public class Productos
    {
        //Propiedades.
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Nombre { get; set; }

        [Column(TypeName = "float")]
        public double Precio { get; set; }

        [StringLength(255)]
        public string Categoria { get; set; }
    }
}
