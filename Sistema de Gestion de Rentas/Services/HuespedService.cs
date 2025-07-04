using Sistema_de_Gestion_de_Rentas.Data;
using System;

namespace Sistema_de_Gestion_de_Rentas.Services
{
    public static class HuespedService
    {
        public static void Inicializar()
        {
            // Aqu� no es necesario crear la tabla si ya existe, as� que eliminamos este llamado.
            // HuespedDAO.CrearTablaHuespedes(); // Esto puede eliminarse
        }

        public static void GuardarHuesped(
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
            // Validaci�n m�nima
            if (string.IsNullOrWhiteSpace(identificacion) ||
                string.IsNullOrWhiteSpace(usuario) ||
                string.IsNullOrWhiteSpace(contrasena) ||
                string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(primerApellido))
            {
                throw new ArgumentException("Todos los campos obligatorios deben completarse.");
            }

            try
            {
                // Llamamos al m�todo sincr�nico para insertar el hu�sped
                HuespedDAO.InsertarHuesped(
                    identificacion, usuario, contrasena, nombre,
                    primerApellido, segundoApellido, correo, telefono, paisOrigen
                );
            }
            catch (ArgumentException ex)
            {
                // Si el usuario, nombre o correo est�n duplicados, capturamos la excepci�n
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                // Capturamos otros errores generales
                throw new Exception("Error al guardar el hu�sped: " + ex.Message);
            }
        }

        public static bool VerificarCredenciales(string usuario, string contrasena)
        {
            // Verifica si las credenciales son correctas
            return HuespedDAO.ValidarLogin(usuario, contrasena);
        }

        public static bool EsAdmin(string usuario)
        {
            // Comprobar si el usuario tiene el rol de Administrador
            try
            {
                var huesped = HuespedDAO.ObtenerHuespedPorUsuario(usuario);

                // Verificamos si el rol es "admin"
                return huesped != null && huesped.Rol == "admin"; // Ahora se verifica el rol directamente
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar el rol de usuario: " + ex.Message);
            }
        }

        // Nuevo m�todo para obtener el rol de un usuario
        public static string ObtenerRolUsuario(string usuario)
        {
            try
            {
                var huesped = HuespedDAO.ObtenerHuespedPorUsuario(usuario);
                if (huesped == null)
                {
                    throw new Exception("Usuario no encontrado.");
                }

                return huesped.Rol; // Devuelve el rol del usuario
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el rol de usuario: " + ex.Message);
            }
        }

        // M�todo que validar� la disponibilidad del usuario, ahora usando el m�todo sincr�nico
        public static bool ValidarDisponibilidadUsuario(string usuario)
        {
            try
            {
                var huesped = HuespedDAO.ObtenerHuespedPorUsuario(usuario);
                return huesped == null; // Si el usuario no existe, es v�lido
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar la disponibilidad del usuario: " + ex.Message);
            }
        }
    }
}
