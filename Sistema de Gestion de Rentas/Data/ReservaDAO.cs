using Npgsql;
using Sistema_de_Gestion_de_Rentas.Reservas;
using System;
using System.Collections.Generic;

namespace Sistema_de_Gestion_de_Rentas.Data
{
    public static class ReservaDAO
    {
        // Método para obtener el nombre del lugar según el hospedaje_id
        public static string ObtenerLugarPorHospedajeId(int hospedajeId)
        {
            string lugar = string.Empty;

            try
            {
                Conexion conexion = new Conexion();
                using (var conn = conexion.ObtenerConexion())
                {
                    string sql = "SELECT nombre FROM hospedajes WHERE id = @hospedajeId";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@hospedajeId", hospedajeId);

                        var result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            lugar = result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error al obtener lugar: {ex.Message}");
            }

            return lugar;
        }

        // Método para obtener reservas por huesped_id
        public static List<ReservaHistorialDTO> ObtenerReservasPorHuespedId(int huespedId)
        {
            var reservas = new List<ReservaHistorialDTO>();

            try
            {
                Conexion conexion = new Conexion();
                using (var conn = conexion.ObtenerConexion())
                {
                    string sql = @"
                        SELECT * FROM reservas
                        WHERE huesped_id = @huespedId
                        ORDER BY fecha_entrada DESC;
                    ";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@huespedId", huespedId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                reservas.Add(new ReservaHistorialDTO
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    HuespedId = reader.GetInt32(reader.GetOrdinal("huesped_id")),  // Asegúrate de que sea GetInt32 aquí
                                    HospedajeId = reader.GetInt32(reader.GetOrdinal("hospedaje_id")),
                                    FechaEntrada = reader.GetDateTime(reader.GetOrdinal("fecha_entrada")),
                                    FechaSalida = reader.GetDateTime(reader.GetOrdinal("fecha_salida")),
                                    CantidadPersonas = reader.GetInt32(reader.GetOrdinal("cantidad_personas")),
                                    CantidadNoches = reader.GetInt32(reader.GetOrdinal("cantidad_noches")),
                                    Estado = reader.GetBoolean(reader.GetOrdinal("estado"))
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error al obtener reservas: {ex.Message}");
            }

            return reservas;
        }
        public static bool ActualizarEstadoReserva(int reservaId, bool nuevoEstado)
        {
            var conexion = new Conexion();
            bool actualizacionExitosa = false;

            try
            {
                // Intentamos obtener el Id como int
                int huespedId = SesionUsuario.ObtenerIdComoInt();  // Conversión correcta

                // Verificamos que el huespedId es válido
                if (huespedId <= 0)
                {
                    throw new InvalidOperationException("El ID del huésped no es válido.");
                }

                conexion.UsarConexion(conn =>
                {
                    // Creamos la consulta SQL para actualizar el estado de la reserva
                    string query = "UPDATE Reservas SET Estado = @Estado WHERE Id = @Id";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        // Agregamos los parámetros necesarios
                        cmd.Parameters.AddWithValue("@Estado", nuevoEstado);
                        cmd.Parameters.AddWithValue("@Id", reservaId);  // Aquí pasamos el reservaId como int

                        // Ejecutamos el comando y verificamos el número de filas afectadas
                        int filasAfectadas = cmd.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            actualizacionExitosa = true;
                        }
                    }
                });

                return actualizacionExitosa;
            }
            catch (Exception ex)
            {
                // Si ocurre un error, lo capturamos
                Console.WriteLine($"Error al actualizar el estado de la reserva: {ex.Message}");
                return false;
            }
        }

    }
}
