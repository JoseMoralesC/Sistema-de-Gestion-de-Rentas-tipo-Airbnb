using Npgsql;
using System;
using System.Threading.Tasks;

namespace Sistema_de_Gestion_de_Rentas.Data
{
    public class Conexion
    {
        // Cadena de conexión a la base de datos PostgreSQL
        public string connectionString = "Host=metro.proxy.rlwy.net;Port=19119;Username=postgres;Password=NPJusWKFapqvTFuMBchlOIuKQaWwtecg;Database=railway";

        // Método para obtener la conexión de manera sincrónica
        public NpgsqlConnection ObtenerConexion()
        {
            try
            {
               
                var conn = new NpgsqlConnection(connectionString);

                
                conn.Open();

                return conn;
            }
            catch (Exception ex)
            {
                // Capturar el error si la conexión falla
                Console.WriteLine($"Error al intentar conectar a la base de datos: {ex.Message}");
                throw new Exception("No se pudo establecer una conexión con la base de datos.");
            }
        }

        // Método asincrónico para obtener la conexión
        public async Task<NpgsqlConnection> ObtenerConexionAsync()
        {
            try
            {
                // Crear la conexión
                var conn = new NpgsqlConnection(connectionString);

                // Intentar abrir la conexión de manera asincrónica
                await conn.OpenAsync();

                return conn;
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error al intentar conectar a la base de datos: {ex.Message}");
                throw new Exception("No se pudo establecer una conexión con la base de datos.");
            }
        }

        
        public void UsarConexion(Action<NpgsqlConnection> accion)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    accion(conn);  
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error durante la operación de base de datos: {ex.Message}");
                
                throw;
            }
        }

        
        public async Task UsarConexionAsync(Func<NpgsqlConnection, Task> accionAsync)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    await accionAsync(conn);  
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error durante la operación de base de datos: {ex.Message}");
                throw new Exception("Error al ejecutar la operación asincrónica con la base de datos.");
            }
        }
    }
}
