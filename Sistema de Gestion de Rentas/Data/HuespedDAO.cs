using Npgsql;
using System;
using System.Windows.Forms;

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

                    MessageBox.Show("Tabla 'Huespedes' creada correctamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear la tabla:\n" + ex.Message);
            }
        }
    }
}
