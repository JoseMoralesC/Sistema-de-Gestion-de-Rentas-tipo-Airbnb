using Npgsql;
using System;

namespace Sistema_de_Gestion_de_Rentas.Data
{
    public class HuespedDAO
    {
        // Método para crear la tabla de huéspedes
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
                            pais_origen VARCHAR(50),
                            rol VARCHAR(20) DEFAULT 'huesped'
                        );
                    ";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error al crear tabla: {ex.Message}");
            }
        }

        // Método para verificar si el usuario, nombre o correo ya existen en la base de datos
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
                return true; // Consideramos que existe si ocurre un error.
            }
        }

        // Método para insertar un nuevo huésped
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
            string rol = "huesped") // Asignamos el rol por defecto
        {
            // Validar que no se use el nombre de usuario "admin"
            if (usuario.ToLower() == "admin")
            {
                throw new ArgumentException("El nombre de usuario 'Admin' está reservado.");
            }

            // Verificar si ya existe un admin en la base de datos
            if (rol.ToLower() == "admin")
            {
                if (ExisteAdmin())
                {
                    throw new ArgumentException("Ya existe un usuario con el rol de Admin en el sistema.");
                }
            }

            // Verificar si el usuario, nombre o correo ya están registrados
            if (VerificarUsuarioExistente(usuario, nombre, correo))
            {
                throw new ArgumentException("El nombre de usuario, nombre o correo ya están registrados.");
            }

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

        // Método para verificar si ya existe un usuario con el rol de Admin en la base de datos
        private static bool ExisteAdmin()
        {
            try
            {
                Conexion conexion = new Conexion();
                using (var conn = conexion.ObtenerConexion())
                {
                    string sql = @"
                        SELECT COUNT(*) FROM Huespedes
                        WHERE rol = 'admin';
                    ";

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
                return true; // Consideramos que existe si ocurre un error
            }
        }

        // Método para validar el login de un usuario
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

        // Método para obtener los detalles completos de un huésped por su nombre de usuario
        public static Huesped ObtenerHuespedPorUsuario(string usuario)
        {
            try
            {
                Conexion conexion = new Conexion();
                using (var conn = conexion.ObtenerConexion())
                {
                    string sql = @"
                        SELECT * FROM Huespedes
                        WHERE usuario = @usuario
                        LIMIT 1;
                    ";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@usuario", usuario);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Huesped(
                                    reader.GetString(reader.GetOrdinal("identificacion")),

                                    reader.GetString(reader.GetOrdinal("usuario")),
                                    reader.GetString(reader.GetOrdinal("contrasena")),
                                    reader.GetString(reader.GetOrdinal("nombre")),
                                    reader.GetString(reader.GetOrdinal("primer_apellido")),
                                    reader.IsDBNull(reader.GetOrdinal("segundo_apellido")) ? null : reader.GetString(reader.GetOrdinal("segundo_apellido")),
                                    reader.IsDBNull(reader.GetOrdinal("correo")) ? null : reader.GetString(reader.GetOrdinal("correo")),
                                    reader.IsDBNull(reader.GetOrdinal("telefono")) ? null : reader.GetString(reader.GetOrdinal("telefono")),
                                    reader.IsDBNull(reader.GetOrdinal("pais_origen")) ? null : reader.GetString(reader.GetOrdinal("pais_origen")),
                                    reader.GetString(reader.GetOrdinal("rol"))
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
    }
}
