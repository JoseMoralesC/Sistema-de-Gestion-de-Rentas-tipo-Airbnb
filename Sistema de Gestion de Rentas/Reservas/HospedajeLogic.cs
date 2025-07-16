using Sistema_de_Gestion_de_Rentas.Data;
using Npgsql;
using System;
using System.Collections.Generic;

namespace Sistema_de_Gestion_de_Rentas.Reservas
{
    public static class HospedajeLogic
    {
        private static readonly Conexion _conexion = new Conexion();

        public static Hospedaje ObtenerHospedajePorID(int id)
        {
            Hospedaje hospedaje = null;
            string query = @"
                SELECT id, nombre, ubicacion, precio_por_noche, capacidad_personas, habitaciones, descripcion
                FROM hospedajes
                WHERE id = @id AND estado = true
            ";

            _conexion.UsarConexion(conn =>
            {
                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    hospedaje = new Hospedaje
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Ubicacion = reader.GetString(2),
                        PrecioPorNoche = reader.GetDecimal(3),
                        CapacidadPersonas = reader.GetInt32(4),
                        Habitaciones = reader.GetInt32(5),
                        Descripcion = reader.GetString(6)
                    };
                }
            });

            return hospedaje;
        }

        public static List<Hospedaje> ObtenerHospedajesParaContenedores(string provincia, List<int> idsHospedajes)
        {
            var lista = new List<Hospedaje>();
            string query = @"
        SELECT id, nombre, ubicacion, precio_por_noche, capacidad_personas, habitaciones, descripcion
        FROM hospedajes
        WHERE lower(ubicacion) = lower(@provincia)
          AND id = ANY(@ids)
          AND estado = true
    ";

            _conexion.UsarConexion(conn =>
            {
                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@provincia", provincia);

                var idsArray = idsHospedajes.ToArray();
                var idsParam = cmd.Parameters.AddWithValue("@ids", idsArray);
                idsParam.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Integer;

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Hospedaje
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Ubicacion = reader.GetString(2),
                        PrecioPorNoche = reader.GetDecimal(3),
                        CapacidadPersonas = reader.GetInt32(4),
                        Habitaciones = reader.GetInt32(5),
                        Descripcion = reader.GetString(6)
                    });
                }
            });

            return lista;
        }

    }

    public class Hospedaje
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Ubicacion { get; set; }
        public decimal PrecioPorNoche { get; set; }
        public int CapacidadPersonas { get; set; }
        public int Habitaciones { get; set; }
        public string Descripcion { get; set; }
    }
}
