using Npgsql;
using System;

namespace Sistema_de_Gestion_de_Rentas.Data
{
    public class HuespedDAO
    {
        public static string ObtenerRolPorUsuarioYContrasena(string usuario, string contrasena)
        {
            try
            {
                Conexion conexion = new Conexion();
                using (var conn = conexion.ObtenerConexion())
                {
                    string sql = @"
                        SELECT rol FROM Huespedes
                        WHERE usuario = @usuario AND contrasena = @contrasena
                        LIMIT 1;
                    ";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@usuario", usuario);
                        cmd.Parameters.AddWithValue("@contrasena", contrasena);

                        var result = cmd.ExecuteScalar();
                        return result?.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error al obtener rol por usuario y contraseña: {ex.Message}");
                return null;
            }
        }

        public static Huesped ObtenerHuespedPorUsuarioYContrasena(string usuario, string contrasena)
        {
            try
            {
                Conexion conexion = new Conexion();
                using (var conn = conexion.ObtenerConexion())
                {
                    string sql = @"
                        SELECT * FROM Huespedes
                        WHERE usuario = @usuario AND contrasena = @contrasena
                        LIMIT 1;
                    ";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@usuario", usuario);
                        cmd.Parameters.AddWithValue("@contrasena", contrasena);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Huesped(
                                    identificacion: reader.GetString(reader.GetOrdinal("identificacion")),
                                    usuario: reader.GetString(reader.GetOrdinal("usuario")),
                                    contrasena: reader.GetString(reader.GetOrdinal("contrasena")),
                                    nombre: reader.GetString(reader.GetOrdinal("nombre")),
                                    primerApellido: reader.GetString(reader.GetOrdinal("primer_apellido")),
                                    segundoApellido: reader.IsDBNull(reader.GetOrdinal("segundo_apellido")) ? null : reader.GetString(reader.GetOrdinal("segundo_apellido")),
                                    correo: reader.IsDBNull(reader.GetOrdinal("correo")) ? null : reader.GetString(reader.GetOrdinal("correo")),
                                    telefono: reader.IsDBNull(reader.GetOrdinal("telefono")) ? null : reader.GetString(reader.GetOrdinal("telefono")),
                                    paisOrigen: reader.IsDBNull(reader.GetOrdinal("pais_origen")) ? null : reader.GetString(reader.GetOrdinal("pais_origen")),
                                    rol: reader.GetString(reader.GetOrdinal("rol"))
                                );

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error al obtener huésped por usuario y contraseña: {ex.Message}");
                throw new Exception("Error al obtener el huésped por usuario y contraseña: " + ex.Message);
            }

            return null;
        }

        public static bool VerificarUsuarioExistente(string usuario, string nombre, string correo)
        {
            try
            {
                Conexion conexion = new Conexion();
                using (var conn = conexion.ObtenerConexion())
                {
                    string sql = @"
                        SELECT COUNT(*) FROM Huespedes
                        WHERE usuario = @usuario OR nombre = @nombre OR correo = @correo;
                    ";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@usuario", usuario);
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                        cmd.Parameters.AddWithValue("@correo", correo);

                        long count = (long)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error al verificar usuario existente: {ex.Message}");
                return true;
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
            string paisOrigen,
            string rol = "huesped")
        {
            if (usuario.ToLower() == "admin")
                throw new ArgumentException("El nombre de usuario 'Admin' está reservado.");

            if (rol.ToLower() == "admin" && ExisteAdmin())
                throw new ArgumentException("Ya existe un usuario con el rol de Admin en el sistema.");

            if (VerificarUsuarioExistente(usuario, nombre, correo))
                throw new ArgumentException("El nombre de usuario, nombre o correo ya están registrados.");

            try
            {
                Conexion conexion = new Conexion();
                using (var conn = conexion.ObtenerConexion())
                {
                    string sql = @"
                        INSERT INTO Huespedes (
                            identificacion, usuario, contrasena, nombre,
                            primer_apellido, segundo_apellido,
                            correo, telefono, pais_origen, rol
                        ) VALUES (
                            @identificacion, @usuario, @contrasena, @nombre,
                            @primer_apellido, @segundo_apellido,
                            @correo, @telefono, @pais_origen, @rol
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
                        cmd.Parameters.AddWithValue("@rol", rol);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error al insertar huésped: {ex.Message}");
                throw new Exception("Error al insertar huésped: " + ex.Message);
            }
        }

        private static bool ExisteAdmin()
        {
            try
            {
                Conexion conexion = new Conexion();
                using (var conn = conexion.ObtenerConexion())
                {
                    string sql = @"SELECT COUNT(*) FROM Huespedes WHERE rol = 'admin';";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        long count = (long)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error al verificar si existe un admin: {ex.Message}");
                return true;
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
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error al validar login: {ex.Message}");
                return false;
            }
        }

        public static Huesped ObtenerHuespedPorUsuario(string usuario)
        {
            try
            {
                Conexion conexion = new Conexion();
                using (var conn = conexion.ObtenerConexion())
                {
                    string sql = @"SELECT * FROM Huespedes WHERE usuario = @usuario LIMIT 1;";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@usuario", usuario);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Huesped(
                                  identificacion: reader.GetString(reader.GetOrdinal("identificacion")),
                                  usuario: reader.GetString(reader.GetOrdinal("usuario")),
                                  contrasena: reader.GetString(reader.GetOrdinal("contrasena")),
                                  nombre: reader.GetString(reader.GetOrdinal("nombre")),
                                  primerApellido: reader.GetString(reader.GetOrdinal("primer_apellido")),
                                  segundoApellido: reader.IsDBNull(reader.GetOrdinal("segundo_apellido")) ? null : reader.GetString(reader.GetOrdinal("segundo_apellido")),
                                  correo: reader.IsDBNull(reader.GetOrdinal("correo")) ? null : reader.GetString(reader.GetOrdinal("correo")),
                                  telefono: reader.IsDBNull(reader.GetOrdinal("telefono")) ? null : reader.GetString(reader.GetOrdinal("telefono")),
                                  paisOrigen: reader.IsDBNull(reader.GetOrdinal("pais_origen")) ? null : reader.GetString(reader.GetOrdinal("pais_origen")),
                                  rol: reader.GetString(reader.GetOrdinal("rol"))
                              );

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error al obtener huésped: {ex.Message}");
                throw new Exception("Error al obtener el huésped por usuario: " + ex.Message);
            }

            return null;
        }
        public static bool ActualizarHuesped(Huesped huesped)
        {
            try
            {
                Conexion conexion = new Conexion();
                using (var conn = conexion.ObtenerConexion())
                {
                    string sql = @"
                UPDATE Huespedes SET
                    usuario = @usuario,
                    contrasena = @contrasena,
                    nombre = @nombre,
                    primer_apellido = @primer_apellido,
                    segundo_apellido = @segundo_apellido,
                    correo = @correo,
                    telefono = @telefono,
                    pais_origen = @pais_origen
                WHERE identificacion = @identificacion;
            ";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@usuario", huesped.Usuario);
                        cmd.Parameters.AddWithValue("@contrasena", huesped.Contrasena);
                        cmd.Parameters.AddWithValue("@nombre", huesped.Nombre);
                        cmd.Parameters.AddWithValue("@primer_apellido", huesped.PrimerApellido);
                        cmd.Parameters.AddWithValue("@segundo_apellido", (object)huesped.SegundoApellido ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@correo", (object)huesped.Correo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@telefono", (object)huesped.Telefono ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@pais_origen", (object)huesped.PaisOrigen ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@identificacion", huesped.Identificacion);

                        int filasAfectadas = cmd.ExecuteNonQuery();
                        return filasAfectadas > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error al actualizar huésped: {ex.Message}");
                return false;
            }
        }

    }
}
