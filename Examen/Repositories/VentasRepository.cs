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
    public class VentasRepository : ExamenDbContext, IVentasRepository
    {

        // Metodos.

        // Agegar.
        public bool AddVenta(Ventas venta)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO [ventas] (IDCliente, Fecha, Total) VALUES (@IDCliente, @Fecha, @Total)";
                    command.Parameters.Add("@IDCliente", SqlDbType.Int).Value = venta.IDCliente;
                    command.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = (object)venta.Fecha ?? DBNull.Value;
                    command.Parameters.Add("@Total", SqlDbType.Float).Value = (object)venta.Total ?? DBNull.Value;
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new VentasExceptions("Error al agregar la venta.", ex);
            }
        }

        // Eliminar.
        public bool DeleteVenta(int id)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM [ventas] WHERE ID = @ID";
                    command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new VentasExceptions("Error al borrar la venta.", ex);
            }
        }

        //Obtener todas las ventas.
        public IEnumerable<Ventas> GetAllVentas()
        {
            //variable.
            List<Ventas> ventas = new List<Ventas>();

            try
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM [ventas]";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Ventas venta = new Ventas
                            {
                                ID = Convert.ToInt32(reader[0]),
                                IDCliente = Convert.ToInt32(reader[1]),
                                Fecha = (DateTime)(reader.IsDBNull(2) ? (DateTime?)null : (DateTime)reader["Fecha"]),
                                Total = (double)(reader.IsDBNull(3) ? (double?)null : (double)reader["Total"])
                            };
                            ventas.Add(venta);
                        }
                        reader.Close();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new VentasExceptions("Error al obtener todas las  venta.", ex);
            }

            return ventas;
        }

        // Obtener la venta por id.
        public Ventas GetVentaById(int id)
        {
            //variable.
            Ventas venta = null;

            try
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM [ventas] WHERE ID = @ID";
                    command.Parameters.Add("@ID", SqlDbType.Int).Value = id;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            venta = new Ventas
                            {
                                ID = Convert.ToInt32(reader[0]),
                                IDCliente = Convert.ToInt32(reader[1]),
                                Fecha = Convert.ToDateTime(reader[2]),
                                Total = Convert.ToDouble(reader[3])                                
                            };
                        }
                        reader.Close();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new VentasExceptions("Error al obtener la venta por ID.", ex);
            }

            return venta;
        }

        // Actualizar.
        public bool UpdateVenta(Ventas venta)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "UPDATE [ventas] SET IDCliente = @IDCliente, Fecha = @Fecha, Total = @Total WHERE ID = @ID";
                    command.Parameters.Add("@ID", SqlDbType.Int).Value = venta.ID;
                    command.Parameters.Add("@IDCliente", SqlDbType.Int).Value = venta.IDCliente;
                    command.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = venta.Fecha;
                    command.Parameters.Add("@Total", SqlDbType.Float).Value = venta.Total;
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new VentasExceptions("Error al actualizar la venta.", ex);
            }
        }
        
        // Obtener venta por fecha.
        public IEnumerable<Ventas> GetVentasByDate(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM [ventas] WHERE Fecha BETWEEN @FechaInicio AND @FechaFin";
                    command.Parameters.Add("@FechaInicio", SqlDbType.DateTime).Value = fechaInicio;
                    command.Parameters.Add("@FechaFin", SqlDbType.DateTime).Value = fechaFin;

                    var ventas = new List<Ventas>();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Ventas venta = new Ventas
                            {
                                ID = Convert.ToInt32(reader[0]),
                                IDCliente = Convert.ToInt32(reader[1]),
                                Fecha = (DateTime)(reader.IsDBNull(2) ? null : (DateTime?)reader.GetDateTime(2)),
                                Total = (double)(reader.IsDBNull(3) ? null : (double?)reader.GetDouble(3))
                            };
                            ventas.Add(venta);
                        }
                        reader.Close();
                        connection.Close();
                    }

                    return ventas;
                }
            }
            catch (Exception ex)
            {
                throw new VentasExceptions("Error al obtener ventas por fecha.", ex);
            }
        }

        // Obtener venta por fecha detallada.
        public IEnumerable<(Ventas, Clientes, Productos, VentasItems, double)> GetVentasDetalladas(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                //variable.
                var ventasConDetalles = new List<(Ventas, Clientes, Productos, VentasItems, double)>();

                using (var connection = GetConnection())
                {
                    connection.Open();

                    using (var command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "SELECT V.*, C.*, P.*, VI.PrecioTotal, VI.Cantidad " +
                                              "FROM [Ventas] V " +
                                              "JOIN [Clientes] C ON V.IDCliente = C.ID " +
                                              "JOIN [VentasItems] VI ON V.ID = VI.IDVenta " +
                                              "JOIN [Productos] P ON VI.IDProducto = P.ID " +
                                              "WHERE V.Fecha BETWEEN @FechaInicio AND @FechaFin";
                        command.Parameters.Add("@FechaInicio", SqlDbType.DateTime).Value = fechaInicio;
                        command.Parameters.Add("@FechaFin", SqlDbType.DateTime).Value = fechaFin;

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var venta = new Ventas
                                    {
                                        ID = Convert.ToInt32(reader["ID"]),
                                        IDCliente = Convert.ToInt32(reader["IDCliente"]),
                                        Fecha = Convert.ToDateTime(reader["Fecha"]),
                                        Total = Convert.ToDouble(reader["Total"])
                                    };

                                    var cliente = new Clientes
                                    {
                                        ID = Convert.ToInt32(reader["IDCliente"]),
                                        Cliente = Convert.ToString(reader["Nombre"]),
                                        Telefono = Convert.ToString(reader["Telefono"]),
                                        Correo = Convert.ToString(reader["Mail"])
                                    };

                                    var producto = new Productos
                                    {
                                        ID = Convert.ToInt32(reader["IDProducto"]),
                                        Nombre = Convert.ToString(reader["Nombre"]),
                                        Precio = Convert.ToInt32(reader["Pecio"]),
                                        Categoria = Convert.ToString(reader["Nombre"])
                                    };

                                    var ventasItems = new VentasItems
                                    {
                                        Cantidad = Convert.ToDouble(reader["Cantidad"])
                                    };

                                    var importe = Convert.ToDouble(reader["PrecioTotal"]);

                                    ventasConDetalles.Add((venta, cliente, producto, ventasItems, importe));
                                }
                                reader.Close();
                                connection.Close();
                            }
                        }
                    }
                }

                return ventasConDetalles;
            }
            catch (Exception ex)
            {
                throw new VentasExceptions("Error al obtener ventas con detalles.", ex);
            }
        }       

        // Obtener total.
        public double GetTotalVentas(int idVenta)
        {
            // Variable.
            double totalVenta = 0;

            try
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT SUM(PrecioTotal) FROM [ventasitems] WHERE IDVenta = @IDVenta";
                    command.Parameters.AddWithValue("@IDVenta", idVenta);

                    object result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        totalVenta = Convert.ToDouble(result);
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new VentasExceptions("Error al calcular el total de la venta.", ex);
            }

            return totalVenta;
        }

        // Obtener ultima venta.
        public int GetUltimaVenta()
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT MAX(ID) FROM [Ventas]";

                    var result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        
                        return 0;
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw new VentasExceptions("Error al obtener el ID de la última venta.", ex);
            }
        }
    }
    
}
