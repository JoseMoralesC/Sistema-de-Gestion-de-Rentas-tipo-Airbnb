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
                // Crear la conexión
                var conn = new NpgsqlConnection(connectionString);

                // Intentar abrir la conexión
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
                // Capturar el error si la conexión falla
                Console.WriteLine($"Error al intentar conectar a la base de datos: {ex.Message}");
                throw new Exception("No se pudo establecer una conexión con la base de datos.");
            }
        }

        // Método para obtener una conexión utilizando 'using' para garantizar el cierre automático
        public void UsarConexion(Action<NpgsqlConnection> accion)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    accion(conn);  // Ejecutar la acción proporcionada con la conexión abierta
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error durante la operación de base de datos: {ex.Message}");
                throw new Exception("Error al ejecutar la operación con la base de datos.");
            }
        }

        // Método asincrónico para ejecutar una operación utilizando 'using'
        public async Task UsarConexionAsync(Func<NpgsqlConnection, Task> accionAsync)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    await accionAsync(conn);  // Ejecutar la acción asincrónica proporcionada con la conexión abierta
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
