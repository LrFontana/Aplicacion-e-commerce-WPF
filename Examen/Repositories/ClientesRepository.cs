using Examen.HanlderExceptions;
using Examen.Models;
using Examen.Repositories.ExamenDb;
using Examen.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Repositories
{
    public class ClientesRepository : ExamenDbContext, IClienteRepository
    {
        // Metodos.

        // Agregar.
        public bool AddCliente(Clientes clienteModel)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO [clientes] (Cliente, Telefono, Correo) VALUES (@Cliente, @Telefono, @Correo)";
                    command.Parameters.Add("@Cliente", SqlDbType.NVarChar).Value = clienteModel.Cliente;
                    command.Parameters.Add("@Telefono", SqlDbType.NVarChar).Value = (object)clienteModel.Telefono ?? DBNull.Value;
                    command.Parameters.Add("@Correo", SqlDbType.NVarChar).Value = (object)clienteModel.Correo ?? DBNull.Value;
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new ClientesExceptions("Error al agregar el cliente.", ex);
            }            

        }

        // Autenticar.
        public bool AuthenticateCliente(NetworkCredential crendential)
        {
            //Variable.
            bool validCliente;

            using (var connection  = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM [clientes] where Cliente=@Cliente and [Correo]=@Mail";
                command.Parameters.Add("@Cliente", SqlDbType.NVarChar).Value=crendential.UserName;
                command.Parameters.Add("@Mail", SqlDbType.NVarChar).Value = crendential.Password;
                validCliente = command.ExecuteScalar() == null ? false : true;
                connection.Close();
            }
            return validCliente;
        }


        // Eliminar.
        public bool DeleteCliente(int id)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM [clientes] WHERE ID = @ID";
                    command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new ClientesExceptions("Error al borrar el cliente.", ex);
                
            }
        }

        // Obtener todos los clientes.
        public IEnumerable<Clientes> GetAllClientes()
        {
            try
            {
                List<Clientes> clientes = new List<Clientes>();
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM [clientes] ORDER BY Cliente";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Clientes cliente = new Clientes
                            {
                                ID = Convert.ToInt32(reader[0]),
                                Cliente = reader[1].ToString(),
                                Telefono = reader[2].ToString(),
                                Correo = reader[3].ToString()
                            };
                            clientes.Add(cliente);
                        }
                        reader.Close();
                        connection.Close();
                    }
                }
                return clientes;
            }
            catch (Exception ex)
            {

                throw new ClientesExceptions("Error al obtener todos los clientes.", ex);
            }
        }

        // Obternet el cliente por id.
        public Clientes GetClienteById(int id)
        {
            try
            {
                Clientes cliente = null;
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM [clientes] where ID=@ID";
                    command.Parameters.Add("@ID", SqlDbType.NVarChar).Value = id;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cliente = new Clientes
                            {
                                ID = Convert.ToInt32(reader[0]),
                                Cliente = reader[1].ToString(),
                                Telefono = reader[2].ToString(),
                                Correo = reader[3].ToString()

                            };
                        }
                        reader.Close();
                        connection.Close();
                    }
                }
                return cliente;
            }
            catch (Exception ex)
            {

                throw new ClientesExceptions("Error al obtener el cliente.", ex);
            }           
           
        }

        // Obtener el cliente por nombre.
        public Clientes GetClienteByName(string clienteNombre)
        {
            Clientes cliente =null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM [clientes] where Cliente=@Cliente";
                    command.Parameters.Add("@Cliente", SqlDbType.NVarChar).Value = clienteNombre;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cliente = new Clientes
                            {
                                ID = Convert.ToInt32(reader[0]),
                                Cliente = reader[1].ToString(),
                                Telefono = reader[2].ToString(),
                                Correo = reader[3].ToString()

                            };
                        }
                        reader.Close();
                        connection.Close();
                    }
                }
                return cliente;
            }
            catch (Exception ex)
            {

                throw new ClientesExceptions("Error al obtener el cliente por nombre.", ex);
            }           
            
        }

        // Actualizar.
        public bool UpdateCliente(Clientes clienteModel)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "UPDATE [clientes] SET Cliente = @Cliente, Telefono = @Telefono, Correo = @Correo WHERE ID = @ID";
                    command.Parameters.Add("@ID", SqlDbType.Int).Value = clienteModel.ID;
                    command.Parameters.Add("@Cliente", SqlDbType.NVarChar).Value = clienteModel.Cliente;
                    command.Parameters.Add("@Telefono", SqlDbType.NVarChar).Value = (object)clienteModel.Telefono ?? DBNull.Value;
                    command.Parameters.Add("@Correo", SqlDbType.NVarChar).Value = (object)clienteModel.Correo ?? DBNull.Value;
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new ClientesExceptions("Error al actualizar el cliente.", ex);
                
            }
        }

        // Buscar.
        public IEnumerable<Clientes> SearchClientes(string nombre = null, string telefono = null, string correo = null)
        {
            try
            {
                List<Clientes> clientes = new List<Clientes>();
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;

                    string query = "SELECT * FROM [clientes] WHERE 1=1";

                    if (!string.IsNullOrWhiteSpace(nombre))
                    {
                        query += " AND Cliente LIKE @Nombre";
                        command.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = "%" + nombre + "%";
                    }

                    if (!string.IsNullOrWhiteSpace(telefono))
                    {
                        query += " AND Telefono LIKE @Telefono";
                        command.Parameters.Add("@Telefono", SqlDbType.NVarChar).Value = "%" + telefono + "%";
                    }

                    if (!string.IsNullOrWhiteSpace(correo))
                    {
                        query += " AND Correo LIKE @Correo";
                        command.Parameters.Add("@Correo", SqlDbType.NVarChar).Value = "%" + correo + "%";
                    }

                    // Incluir la cláusula ORDER BY en la consulta
                    query += " ORDER BY Cliente";

                    command.CommandText = query;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Clientes cliente = new Clientes
                            {
                                ID = Convert.ToInt32(reader[0]),
                                Cliente = reader[1].ToString(),
                                Telefono = reader[2].ToString(),
                                Correo = reader[3].ToString()
                            };
                            clientes.Add(cliente);
                        }
                        reader.Close();
                        connection.Close();
                    }
                }
                return clientes;
            }
            catch (Exception ex)
            {
                throw new ClientesExceptions("Error al buscar los clientes.", ex);
            }
        }
    }
}
