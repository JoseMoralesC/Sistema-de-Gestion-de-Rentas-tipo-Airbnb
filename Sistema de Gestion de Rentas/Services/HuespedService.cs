using Sistema_de_Gestion_de_Rentas.Data;
using System;

namespace Sistema_de_Gestion_de_Rentas.Services
{
    public static class HuespedService
    {
        public static void Inicializar()
        {
            HuespedDAO.CrearTablaHuespedes();
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
            // Validación mínima
            if (string.IsNullOrWhiteSpace(identificacion) ||
                string.IsNullOrWhiteSpace(usuario) ||
                string.IsNullOrWhiteSpace(contrasena) ||
                string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(primerApellido))
            {
                throw new ArgumentException("Todos los campos obligatorios deben completarse.");
            }

            HuespedDAO.InsertarHuesped(
                identificacion, usuario, contrasena, nombre,
                primerApellido, segundoApellido, correo, telefono, paisOrigen
            );
        }

        public static bool VerificarCredenciales(string usuario, string contrasena)
        {
            return HuespedDAO.ValidarLogin(usuario, contrasena);
        }
    }
}
