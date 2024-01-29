using Examen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Repositories.IRepositories
{
    public interface IVentasItemsRepository
    {
        //Propiedades.
        IEnumerable<VentasItems> GetAllVentasItems();
        IEnumerable<VentasItems> GetVentasItemsByIdVenta(int idVenta);
        VentasItems GetVentasItemById(int id);
        bool AddVentasItem(VentasItems ventasItem);
        bool UpdateVentasItem(VentasItems ventasItem);
        bool DeleteVentasItem(int id);
        double GetPrecioUnitario(int idProducto);
    }
}
