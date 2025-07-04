using Sistema_de_Gestion_de_Rentas.Data;
using System;

namespace Sistema_de_Gestion_de_Rentas.Services
{
    public static class HuespedService
    {
        // Nuevo método para verificar las credenciales y obtener el rol en una sola operación
        public static string VerificarCredencialesYObtenerRol(string usuario, string contrasena)
        {
            try
            {
                // Llamamos al método de HuespedDAO para obtener el rol por usuario y contrasena
                return HuespedDAO.ObtenerRolPorUsuarioYContrasena(usuario, contrasena);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar las credenciales: " + ex.Message);
            }
        }

        // Método para guardar un nuevo huésped
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
            // Validación mínima
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
                // Llamamos al método sincrónico para insertar el huésped
                HuespedDAO.InsertarHuesped(
                    identificacion, usuario, contrasena, nombre,
                    primerApellido, segundoApellido, correo, telefono, paisOrigen
                );
            }
            catch (ArgumentException ex)
            {
                // Si el usuario, nombre o correo están duplicados, capturamos la excepción
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                // Capturamos otros errores generales
                throw new Exception("Error al guardar el huésped: " + ex.Message);
            }
        }

        // Método que valida las credenciales de inicio de sesión
        public static bool VerificarCredenciales(string usuario, string contrasena)
        {
            // Verifica si las credenciales son correctas
            return HuespedDAO.ValidarLogin(usuario, contrasena);
        }

        // Método que obtiene el rol de un usuario
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

        // Método que valida si el usuario tiene rol de administrador
        public static bool EsAdmin(string usuario)
        {
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

        // Método que validará la disponibilidad del usuario, ahora usando el método sincrónico
        public static bool ValidarDisponibilidadUsuario(string usuario)
        {
            try
            {
                var huesped = HuespedDAO.ObtenerHuespedPorUsuario(usuario);
                return huesped == null; // Si el usuario no existe, es válido
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar la disponibilidad del usuario: " + ex.Message);
            }
        }
    }
}
