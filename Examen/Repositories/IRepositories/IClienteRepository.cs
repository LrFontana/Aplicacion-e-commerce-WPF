using Examen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Repositories.IRepositories
{
    public interface IClienteRepository
    {
        //Propiedades.
        bool AuthenticateCliente(NetworkCredential crendential);
        bool AddCliente(Clientes clienteModel);
        bool UpdateCliente(Clientes clienteModel);
        bool DeleteCliente(int id);
        Clientes GetClienteById(int id);
        Clientes GetClienteByName(string clienteNombre);
        IEnumerable<Clientes> GetAllClientes();
        IEnumerable<Clientes> SearchClientes(string nombre = null, string telefono = null, string correo = null);
    }
}
