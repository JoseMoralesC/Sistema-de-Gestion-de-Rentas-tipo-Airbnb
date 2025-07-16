using Npgsql;
using Sistema_de_Gestion_de_Rentas.Data;
using Sistema_de_Gestion_de_Rentas.Reservas;
using System;

namespace Sistema_de_Gestion_de_Rentas.Services
{
    public class ReservaHospedajeService
    {
        private readonly Conexion _conexion;

        public ReservaHospedajeService()
        {
            _conexion = new Conexion();
        }

        public bool VerificarConexion()
        {
            try
            {
                using (var conn = new NpgsqlConnection(_conexion.connectionString))
                {
                    conn.Open();
                    Console.WriteLine("Conexión exitosa a la base de datos.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al intentar conectar a la base de datos: {ObtenerMensajeCompletoExcepcion(ex)}");
                return false;
            }
        }

        private string ObtenerMensajeCompletoExcepcion(Exception ex)
        {
            string mensaje = ex.Message;
            Exception inner = ex.InnerException;
            while (inner != null)
            {
                mensaje += "\n-> " + inner.Message;
                inner = inner.InnerException;
            }
            return mensaje;
        }

        public void CrearReserva(Reserva reserva)
        {
            if (!VerificarConexion())
                throw new Exception("No se pudo establecer una conexión con la base de datos.");

            string query = @"
        INSERT INTO reservas 
        (huesped_id, hospedaje_id, fecha_entrada, fecha_salida, cantidad_personas, cantidad_noches, estado)
        VALUES 
        (@huesped_id, @hospedaje_id, @fecha_entrada, @fecha_salida, @cantidad_personas, @cantidad_noches, @estado)";

            _conexion.UsarConexion(conn =>
            {
                try
                {
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@huesped_id", reserva.HuespedId);
                        cmd.Parameters.AddWithValue("@hospedaje_id", reserva.HospedajeId);
                        cmd.Parameters.AddWithValue("@fecha_entrada", reserva.FechaEntrada);
                        cmd.Parameters.AddWithValue("@fecha_salida", reserva.FechaSalida);
                        cmd.Parameters.AddWithValue("@cantidad_personas", reserva.CantidadPersonas);
                        cmd.Parameters.AddWithValue("@cantidad_noches", reserva.CantidadNoches);
                        cmd.Parameters.AddWithValue("@estado", true); 

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR GENERAL] {ex.Message}");
                    if (ex.InnerException != null)
                        Console.WriteLine($"[INNER] {ex.InnerException.Message}");
                    throw;
                }
            });
        }


        public void GuardarHospedaje(int provinciaId, string nombre, string ubicacion, decimal precioPorNoche, int capacidadPersonas, int habitaciones, string descripcion)
        {
            string query = @"
                INSERT INTO hospedajes (provincia_id, nombre, ubicacion, precio_por_noche, capacidad_personas, habitaciones, descripcion)
                VALUES (@provincia_id, @nombre, @ubicacion, @precio_por_noche, @capacidad_personas, @habitaciones, @descripcion)";

            _conexion.UsarConexion(conn =>
            {
                try
                {
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@provincia_id", provinciaId);
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                        cmd.Parameters.AddWithValue("@ubicacion", ubicacion);
                        cmd.Parameters.AddWithValue("@precio_por_noche", precioPorNoche);
                        cmd.Parameters.AddWithValue("@capacidad_personas", capacidadPersonas);
                        cmd.Parameters.AddWithValue("@habitaciones", habitaciones);
                        cmd.Parameters.AddWithValue("@descripcion", descripcion);

                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Hospedaje guardado exitosamente.");
                    }
                }
                catch (PostgresException ex)
                {
                    Console.WriteLine($"PostgreSQL ERROR: {ex.Message}");
                    Console.WriteLine($"SQLState: {ex.SqlState}");
                    Console.WriteLine($"Detalle: {ex.Detail}");
                    Console.WriteLine($"Where: {ex.Where}");
                    throw;
                }
            });
        }
    }
}
