using Sistema_de_Gestion_de_Rentas.Data;
using System;

namespace Sistema_de_Gestion_de_Rentas.Services
{
    public static class HuespedService
    {
        // M�todo para autenticar usuario y obtener objeto Huesped completo
        public static Huesped AutenticarUsuario(string usuario, string contrasena)
        {
            try
            {
                // M�todo que devuelve el hu�sped si usuario y contrase�a coinciden, o null si no
                return HuespedDAO.ObtenerHuespedPorUsuarioYContrasena(usuario, contrasena);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al autenticar usuario: " + ex.Message);
            }
        }

        // M�todo para guardar un nuevo hu�sped
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
                HuespedDAO.InsertarHuesped(
                    identificacion, usuario, contrasena, nombre,
                    primerApellido, segundoApellido, correo, telefono, paisOrigen
                );
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar el hu�sped: " + ex.Message);
            }
        }

        // M�todo que valida las credenciales de inicio de sesi�n (opcional si usas AutenticarUsuario)
        public static bool VerificarCredenciales(string usuario, string contrasena)
        {
            return HuespedDAO.ValidarLogin(usuario, contrasena);
        }

        // M�todo que obtiene el rol de un usuario
        public static string ObtenerRolUsuario(string usuario)
        {
            try
            {
                var huesped = HuespedDAO.ObtenerHuespedPorUsuario(usuario);
                if (huesped == null)
                {
                    throw new Exception("Usuario no encontrado.");
                }
                return huesped.Rol;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el rol de usuario: " + ex.Message);
            }
        }

        // M�todo que valida si el usuario tiene rol de administrador
        public static bool EsAdmin(string usuario)
        {
            try
            {
                var huesped = HuespedDAO.ObtenerHuespedPorUsuario(usuario);
                return huesped != null && huesped.Rol.ToLower() == "admin";
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar el rol de usuario: " + ex.Message);
            }
        }

        // M�todo que valida la disponibilidad del usuario
        public static bool ValidarDisponibilidadUsuario(string usuario)
        {
            try
            {
                var huesped = HuespedDAO.ObtenerHuespedPorUsuario(usuario);
                return huesped == null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar la disponibilidad del usuario: " + ex.Message);
            }
        }

        // M�todo que valida credenciales y devuelve el rol si son correctas, o null si no
        public static string VerificarCredencialesYObtenerRol(string usuario, string contrasena)
        {
            try
            {
                var huesped = HuespedDAO.ObtenerHuespedPorUsuarioYContrasena(usuario, contrasena);
                return huesped?.Rol;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar credenciales y obtener rol: " + ex.Message);
            }
        }
    }
}
