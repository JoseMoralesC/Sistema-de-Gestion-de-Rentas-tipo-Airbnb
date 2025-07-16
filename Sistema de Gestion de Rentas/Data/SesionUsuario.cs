using System;

namespace Sistema_de_Gestion_de_Rentas.Data
{
    public static class SesionUsuario
    {
        public static string Id { get; set; }  // El Id sigue siendo un string
        public static string Usuario { get; set; }
        public static string Nombre { get; set; }
        public static string PrimerApellido { get; set; }
        public static string SegundoApellido { get; set; }
        public static string Correo { get; set; }
        public static string Telefono { get; set; }
        public static string PaisOrigen { get; set; }
        public static string Rol { get; set; }

        // Método para cerrar sesión
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

        // Método que verifica si el usuario está logueado (si Id no es nulo o vacío)
        public static bool EstaLogueado()
        {
            return !string.IsNullOrEmpty(Id);
        }

        // Método para obtener el nombre completo del usuario
        public static string NombreCompleto()
        {
            string segundoApellido = SegundoApellido ?? "";
            return $"{Nombre} {PrimerApellido} {segundoApellido}".Trim();
        }

        // Método que convierte el Id a int de forma segura
        public static int ObtenerIdComoInt()
        {
            // Verifica si el Id es válido (si no es nulo o vacío)
            if (int.TryParse(Id, out int id))
            {
                return id;
            }
            else
            {
                // Si no es un número válido, puedes lanzar una excepción o manejar el error de la manera que prefieras.
                throw new InvalidOperationException("El Id del usuario no es válido.");
            }
        }
        
        // Método que lanza una excepción si el Id es nulo o vacío
        public static void ValidarSesion()
        {
            if (string.IsNullOrEmpty(Id))
            {
                throw new InvalidOperationException("El usuario no está logueado.");
            }
        }
    }
}
