using System;

namespace Sistema_de_Gestion_de_Rentas.Data
{
    public static class SesionUsuario
    {
        public static string Id { get; set; }  // Cambiado a string para coincidir con la identificacion
        public static string Usuario { get; set; }
        public static string Nombre { get; set; }
        public static string PrimerApellido { get; set; }
        public static string SegundoApellido { get; set; }
        public static string Correo { get; set; }
        public static string Telefono { get; set; }
        public static string PaisOrigen { get; set; }
        public static string Rol { get; set; }

        public static void CerrarSesion()
        {
            Id = null;
            Usuario = null;
            Nombre = null;
            PrimerApellido = null;
            SegundoApellido = null;
            Correo = null;
            Telefono = null;
            PaisOrigen = null;
            Rol = null;
        }

        public static bool EstaLogueado() => !string.IsNullOrEmpty(Id);

        public static string NombreCompleto()
        {
            string segundoApellido = SegundoApellido ?? "";
            return $"{Nombre} {PrimerApellido} {segundoApellido}".Trim();
        }
    }
}
