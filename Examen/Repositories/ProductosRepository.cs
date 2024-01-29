using Examen.HanlderExceptions;
using Examen.Models;
using Examen.Repositories.ExamenDb;
using Examen.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Repositories
{
    public class ProductosRepository : ExamenDbContext, IProductosRepository
    {

        // Metodos.

        // Agregar.
        public bool AddProducto(Productos producto)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO [productos] (Nombre, Precio, Categoria) VALUES (@Nombre, @Precio, @Categoria)";
                    command.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = producto.Nombre;
                    command.Parameters.Add("@Precio", SqlDbType.Float).Value = producto.Precio;
                    command.Parameters.Add("@Categoria", SqlDbType.NVarChar).Value = (object)producto.Categoria ?? DBNull.Value;
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new ProductosExceptions("Error al agregar el producto.", ex);
            }
        }


        // Eliminar.
        public bool DeleteProducto(int id)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM [productos] WHERE ID = @ID";
                    command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new ProductosExceptions("Error al borrar el producto.", ex);
            }
        }


        // Obtener todos los productos.
        public IEnumerable<Productos> GetAllProductos()
        {
            List<Productos> productos = new List<Productos>();

            try
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM [productos]";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Productos producto = new Productos
                            {
                                ID = Convert.ToInt32(reader[0]),
                                Nombre = reader[1].ToString(),
                                Precio = Convert.ToDouble(reader[2]),
                                Categoria = reader.IsDBNull(3) ? null : reader[3].ToString()
                            };
                            productos.Add(producto);
                        }
                        reader.Close();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ProductosExceptions("Error al obtener todos los producto.", ex);
            }

            return productos;
        }


        // Obtener el productos por id.
        public Productos GetProductoById(int id)
        {
            Productos producto = null;

            try
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM [productos] WHERE ID = @ID";
                    command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            producto = new Productos
                            {
                                ID = Convert.ToInt32(reader[0]),
                                Nombre = reader[1].ToString(),
                                Precio = Convert.ToDouble(reader[2]),
                                Categoria = reader.IsDBNull(3) ? null : reader[3].ToString()
                            };
                        }
                        reader.Close();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ProductosExceptions("Error al obtener el producto por ID.", ex);
            }

            return producto;
        }


        // Obtener el producto por nombre.
        public Productos GetProductoByName(string productoNombre)
        {
            Productos producto = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM [productos] where Nombre=@Nombre";
                    command.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = productoNombre;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            producto = new Productos
                            {
                                ID = Convert.ToInt32(reader[0]),
                                Nombre = reader[1].ToString(),
                                Precio = Convert.ToDouble(reader[2]),
                                Categoria = reader[3].ToString()

                            };
                        }
                        reader.Close();
                        connection.Close();
                    }
                }
                return producto;
            }
            catch (Exception ex)
            {

                throw new ProductosExceptions("Error al obtener el producto por nombre.", ex);
            }

        }

        // Actualizar.
        public bool UpdateProducto(Productos producto)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "UPDATE [productos] SET Nombre = @Nombre, Precio = @Precio, Categoria = @Categoria WHERE ID = @ID";
                    command.Parameters.Add("@ID", SqlDbType.Int).Value = producto.ID;
                    command.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = producto.Nombre;
                    command.Parameters.Add("@Precio", SqlDbType.Float).Value = producto.Precio;
                    command.Parameters.Add("@Categoria", SqlDbType.NVarChar).Value = (object)producto.Categoria ?? DBNull.Value;
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new ProductosExceptions("Error al actualizar el producto.", ex);
            }
        }
    }
}
