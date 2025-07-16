using Sistema_de_Gestion_de_Rentas.Data;
using System;

namespace Sistema_de_Gestion_de_Rentas.Services
{
    public static class HuespedService
    {
        
        public static Huesped AutenticarUsuario(string usuario, string contrasena)
        {
            try
            {
                
                return HuespedDAO.ObtenerHuespedPorUsuarioYContrasena(usuario, contrasena);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al autenticar usuario: " + ex.Message);
            }
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
                throw new Exception("Error al guardar el huésped: " + ex.Message);
            }
        }

        
        public static bool VerificarCredenciales(string usuario, string contrasena)
        {
            return HuespedDAO.ValidarLogin(usuario, contrasena);
        }

        
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
