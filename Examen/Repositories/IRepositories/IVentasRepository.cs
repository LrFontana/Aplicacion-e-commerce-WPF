using Examen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Repositories.IRepositories
{
    public interface IVentasRepository
    {
        //Propiedades.
        IEnumerable<Ventas> GetAllVentas();
        IEnumerable<Ventas> GetVentasByDate(DateTime fechaInicio, DateTime fechaFin);   
        IEnumerable<(Ventas, Clientes, Productos, VentasItems, double)> GetVentasDetalladas(DateTime fechaInicio, DateTime fechaFin);        
        Ventas GetVentaById(int id);
        bool AddVenta(Ventas venta);
        bool UpdateVenta(Ventas venta);
        bool DeleteVenta(int id);
        int GetUltimaVenta();
        double GetTotalVentas(int idVenta);
    }
}
