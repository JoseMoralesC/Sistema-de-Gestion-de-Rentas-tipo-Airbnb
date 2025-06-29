using Npgsql;
using System;

namespace Sistema_de_Gestion_de_Rentas.Data
{
    public class HuespedDAO
    {
        public static void CrearTablaHuespedes()
        {
            try
            {
                Conexion conexion = new Conexion();
                using (var conn = conexion.ObtenerConexion())
                {
                    string sql = @"
                        CREATE TABLE IF NOT EXISTS Huespedes (
                            identificacion VARCHAR(20) PRIMARY KEY,
                            usuario VARCHAR(30) UNIQUE NOT NULL,
                            contrasena VARCHAR(100) NOT NULL,
                            nombre VARCHAR(50) NOT NULL,
                            primer_apellido VARCHAR(50) NOT NULL,
                            segundo_apellido VARCHAR(50),
                            correo VARCHAR(100),
                            telefono VARCHAR(20),
                            pais_origen VARCHAR(50)
                        );
                    ";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                // Aquí se puede loguear el error si se necesita en el futuro
            }
        }

        public static void InsertarHuesped(
            string identificacion,
            string usuario,
            string contrasena,
            string nombre,
            string primerApellido,
            string segundoApellido,
            string correo,
            string telefono,
            string paisOrigen)
        {
            try
            {
                Conexion conexion = new Conexion();
                using (var conn = conexion.ObtenerConexion())
                {
                    string sql = @"
                        INSERT INTO Huespedes (
                            identificacion, usuario, contrasena, nombre,
                            primer_apellido, segundo_apellido,
                            correo, telefono, pais_origen
                        ) VALUES (
                            @identificacion, @usuario, @contrasena, @nombre,
                            @primer_apellido, @segundo_apellido,
                            @correo, @telefono, @pais_origen
                        );
                    ";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@identificacion", identificacion);
                        cmd.Parameters.AddWithValue("@usuario", usuario);
                        cmd.Parameters.AddWithValue("@contrasena", contrasena);
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                        cmd.Parameters.AddWithValue("@primer_apellido", primerApellido);
                        cmd.Parameters.AddWithValue("@segundo_apellido", (object)segundoApellido ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@correo", (object)correo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@telefono", (object)telefono ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@pais_origen", (object)paisOrigen ?? DBNull.Value);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar huésped: " + ex.Message);
            }
        }

        public static bool ValidarLogin(string usuario, string contrasena)
        {
            try
            {
                Conexion conexion = new Conexion();
                using (var conn = conexion.ObtenerConexion())
                {
                    string sql = @"
                        SELECT COUNT(*) FROM Huespedes
                        WHERE usuario = @usuario AND contrasena = @contrasena;
                    ";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@usuario", usuario);
                        cmd.Parameters.AddWithValue("@contrasena", contrasena);

                        long count = (long)cmd.ExecuteScalar();
                        return count == 1;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
