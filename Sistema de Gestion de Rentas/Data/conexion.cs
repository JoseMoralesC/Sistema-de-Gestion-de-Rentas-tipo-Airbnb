using Npgsql;
using System;

namespace Sistema_de_Gestion_de_Rentas.Data
{
    public class Conexion
    {
        // Conexion a base de datos PostgreSQL
        private string connectionString = "Host=metro.proxy.rlwy.net;Port=19119;Username=postgres;Password=NPJusWKFapqvTFuMBchlOIuKQaWwtecg;Database=railway";


        public NpgsqlConnection ObtenerConexion()
        {
            var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            return conn;
        }
    }
}
