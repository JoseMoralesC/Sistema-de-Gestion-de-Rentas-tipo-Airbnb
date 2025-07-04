using System;

namespace Sistema_de_Gestion_de_Rentas.Data
{
    // Enum para el Rol del Huesped
    public enum RolHuesped
    {
        Huesped,
        Admin
    }

    public class Huesped
    {
        public string Identificacion { get; set; }
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string PaisOrigen { get; set; }
        public string Rol { get; set; }

        // Constructor
        public Huesped(string identificacion, string usuario, string contrasena, string nombre,
                       string primerApellido, string segundoApellido, string correo, string telefono,
                       string paisOrigen, string rol)
        {
            Identificacion = identificacion;
            Usuario = usuario;
            Contrasena = contrasena;
            Nombre = nombre;
            PrimerApellido = primerApellido;
            SegundoApellido = segundoApellido;
            Correo = correo;
            Telefono = telefono;
            PaisOrigen = paisOrigen;
            Rol = rol;
        }

        // Método para validar las propiedades básicas (si se desea)
        public bool ValidarDatos()
        {
            return !string.IsNullOrWhiteSpace(Identificacion) &&
                   !string.IsNullOrWhiteSpace(Usuario) &&
                   !string.IsNullOrWhiteSpace(Contrasena) &&
                   !string.IsNullOrWhiteSpace(Nombre) &&
                   !string.IsNullOrWhiteSpace(PrimerApellido);
        }
    }
}
