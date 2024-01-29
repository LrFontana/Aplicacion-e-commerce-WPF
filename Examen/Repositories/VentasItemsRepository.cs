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
    public class VentasItemsRepository : ExamenDbContext, IVentasItemsRepository
    {

        // Metodos.

        // Agregar.
        public bool AddVentasItem(VentasItems ventasItem)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO [ventasitems] (IDVenta, IDProducto, PrecioUnitario, Cantidad, PrecioTotal) VALUES (@IDVenta, @IDProducto, @PrecioUnitario, @Cantidad, @PrecioTotal)";
                    command.Parameters.Add("@IDVenta", SqlDbType.Int).Value = ventasItem.IDVenta;
                    command.Parameters.Add("@IDProducto", SqlDbType.Int).Value = ventasItem.IDProducto;
                    command.Parameters.Add("@PrecioUnitario", SqlDbType.Float).Value = ventasItem.PrecioUnitario;
                    command.Parameters.Add("@Cantidad", SqlDbType.Float).Value = ventasItem.Cantidad;
                    command.Parameters.Add("@PrecioTotal", SqlDbType.Float).Value = ventasItem.PrecioTotal;
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new VentasItemsExceptions("Error al agregar la venta del item.", ex);
            }
        }


        // Eliminar la venta item por id.
        public bool DeleteVentasItem(int id)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM [ventasitems] WHERE ID = @ID";
                    command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new VentasItemsExceptions("Error al borrar la venta del item.", ex);
            }
        }


        // Obtener todas las ventas items.
        public IEnumerable<VentasItems> GetAllVentasItems()
        {
            //variable.
            List<VentasItems> ventasItems = new List<VentasItems>();

            try
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM [ventasitems] ORDER BY ID DESC";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            VentasItems ventasItem = new VentasItems
                            {
                                ID = Convert.ToInt32(reader[0]),
                                IDVenta = Convert.ToInt32(reader[1]),
                                IDProducto = Convert.ToInt32(reader[2]),
                                PrecioUnitario = Convert.ToDouble(reader[3]),
                                Cantidad = Convert.ToDouble(reader[4]),
                                PrecioTotal = Convert.ToDouble(reader[5])
                            };
                            ventasItems.Add(ventasItem);
                        }
                        reader.Close();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new VentasItemsExceptions("Error al obtener todas las venta del item.", ex);
            }

            return ventasItems;
        }

        // Obtener el Id de ventas.
        public IEnumerable<VentasItems> GetVentasItemsByIdVenta(int idVenta)
        {
            // Variable.
            List<VentasItems> ventasItems = new List<VentasItems>();

            try
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM [ventasitems] WHERE IDVenta = @IDVenta";
                    command.Parameters.Add("@IDVenta", SqlDbType.Int).Value = idVenta;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            VentasItems ventasItem = new VentasItems
                            {
                                ID = Convert.ToInt32(reader[0]),
                                IDVenta = Convert.ToInt32(reader[1]),
                                IDProducto = Convert.ToInt32(reader[2]),
                                PrecioUnitario = Convert.ToDouble(reader[3]),
                                Cantidad = Convert.ToDouble(reader[4]),
                                PrecioTotal = Convert.ToDouble(reader[5])
                            };
                            ventasItems.Add(ventasItem);
                        }
                        reader.Close();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new VentasItemsExceptions("Error al obtener los VentasItems por IDVenta.", ex);
            }

            return ventasItems;
        }


        // Obtener ventas items por id.
        public VentasItems GetVentasItemById(int id)
        {
            //variable.
            VentasItems ventasItem = null;

            try
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM [ventasitems] WHERE ID = @ID";
                    command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ventasItem = new VentasItems
                            {
                                ID = Convert.ToInt32(reader[0]),
                                IDVenta = Convert.ToInt32(reader[1]),
                                IDProducto = Convert.ToInt32(reader[2]),
                                PrecioUnitario = Convert.ToDouble(reader[3]),
                                Cantidad = Convert.ToDouble(reader[4]),
                                PrecioTotal = Convert.ToDouble(reader[5])
                            };
                        }
                        reader.Close();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new VentasItemsExceptions("Error al obtener la venta del item por ID.", ex);
            }

            return ventasItem;
        }

        // Actualizar.
        public bool UpdateVentasItem(VentasItems ventasItem)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "UPDATE [ventasitems] SET IDVenta = @IDVenta, IDProducto = @IDProducto, PrecioUnitario = @PrecioUnitario, Cantidad = @Cantidad, PrecioTotal = @PrecioTotal WHERE ID = @ID";
                    command.Parameters.Add("@ID", SqlDbType.Int).Value = ventasItem.ID;
                    command.Parameters.Add("@IDVenta", SqlDbType.Int).Value = ventasItem.IDVenta;
                    command.Parameters.Add("@IDProducto", SqlDbType.Int).Value = ventasItem.IDProducto;
                    command.Parameters.Add("@PrecioUnitario", SqlDbType.Float).Value = ventasItem.PrecioUnitario;
                    command.Parameters.Add("@Cantidad", SqlDbType.Float).Value = ventasItem.Cantidad;
                    command.Parameters.Add("@PrecioTotal", SqlDbType.Float).Value = ventasItem.PrecioTotal;
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new VentasItemsExceptions("Error al actualizar la venta del item.", ex);
            }
        }


        // Obtener precio unitario desde la tabla productos.
        public double GetPrecioUnitario(int idProducto)
        {
            try
            {
                using (var connection = GetConnection()) 
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT Precio FROM Productos WHERE ID = @ID";
                    command.Parameters.Add("@ID", SqlDbType.Int).Value = idProducto;

                    var result = command.ExecuteScalar();
                    if (result != null && double.TryParse(result.ToString(), out double precioUnitario))
                    {
                        return precioUnitario;
                    }
                    else
                    {                        
                        return 0.0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener el precio unitario: " + ex.Message);
                return 0.0;
            }
        }
        
    }
}
