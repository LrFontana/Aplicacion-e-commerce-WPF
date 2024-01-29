using Examen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Repositories.IRepositories
{
    public interface IProductosRepository
    {
        //Propiedades.
        IEnumerable<Productos> GetAllProductos();
        Productos GetProductoById(int id);
        Productos GetProductoByName(string nombreProducto);
        bool AddProducto(Productos producto);
        bool UpdateProducto(Productos producto);
        bool DeleteProducto(int id);
    }
}
