using System;

namespace Sistema_de_Gestion_de_Rentas.Data
{
    public static class SesionUsuario
    {
        public static string Id { get; set; }   
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

       
        public static bool EstaLogueado()
        {
            return !string.IsNullOrEmpty(Id);
        }

      
        public static string NombreCompleto()
        {
            string segundoApellido = SegundoApellido ?? "";
            return $"{Nombre} {PrimerApellido} {segundoApellido}".Trim();
        }

        public static int ObtenerIdComoInt()
        {
            
            if (int.TryParse(Id, out int id))
            {
                return id;
            }
            else
            {
                
                throw new InvalidOperationException("El Id del usuario no es válido.");
            }
        }
        

        public static void ValidarSesion()
        {
            if (string.IsNullOrEmpty(Id))
            {
                throw new InvalidOperationException("El usuario no está logueado.");
            }
        }
    }
}
