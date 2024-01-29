using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Models
{
    public class VentasItems
    {
        //Propiedades.
        [Key]
        public int ID { get; set; }

        [Column(TypeName = "float")]
        public double PrecioUnitario { get; set; }

        [Column(TypeName = "float")]
        public double Cantidad { get; set; }

        [Column(TypeName = "float")]
        public double PrecioTotal { get; set; }

        public int IDVenta { get; set; }

        [ForeignKey("IDVenta")]
        public virtual Ventas Venta { get; set; }

        public int IDProducto { get; set; }

        [ForeignKey("IDProducto")]
        public virtual Productos Producto { get; set; }
    }
}
