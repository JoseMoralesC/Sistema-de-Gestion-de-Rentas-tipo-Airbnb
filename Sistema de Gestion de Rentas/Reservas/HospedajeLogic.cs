using Sistema_de_Gestion_de_Rentas.Reservas;  // Importamos la clase Hospedaje
using System;
using System.Collections.Generic;

namespace Sistema_de_Gestion_de_Rentas.Reservas
{
    public static class HospedajeLogic
    {
        // Lista estática de hospedajes para Cartago
        private static List<Hospedaje> hospedajes = new List<Hospedaje>
        {
            new Hospedaje { Id = 1, Nombre = "Paraiso", Ubicacion = "Cartago", PrecioPorNoche = 100.00m, CapacidadPersonas = 4, Habitaciones = 2, Descripcion = "Un lugar tranquilo y lleno de naturaleza." },
            new Hospedaje { Id = 2, Nombre = "Ujarras", Ubicacion = "Cartago", PrecioPorNoche = 80.00m, CapacidadPersonas = 3, Habitaciones = 1, Descripcion = "Alojamiento cómodo cerca de la naturaleza." },
            new Hospedaje { Id = 3, Nombre = "Orocí", Ubicacion = "Cartago", PrecioPorNoche = 120.00m, CapacidadPersonas = 5, Habitaciones = 3, Descripcion = "Hospedaje con vistas hermosas al volcán." },
            new Hospedaje { Id = 4, Nombre = "Irazú", Ubicacion = "Cartago", PrecioPorNoche = 150.00m, CapacidadPersonas = 6, Habitaciones = 4, Descripcion = "Un refugio de lujo al pie del volcán Irazú." }
        };

        // Método para obtener hospedajes para los contenedores en la provincia de Cartago, filtrado por IDs
        public static List<Hospedaje> ObtenerHospedajesParaContenedores(string provincia, List<int> idsHospedajes)
        {
            // Filtramos los hospedajes estáticos según la provincia y los IDs solicitados
            var resultado = new List<Hospedaje>();

            foreach (var hospedaje in hospedajes)
            {
                if (hospedaje.Ubicacion.Equals(provincia, StringComparison.OrdinalIgnoreCase) && idsHospedajes.Contains(hospedaje.Id))
                {
                    resultado.Add(hospedaje);
                }
            }

            return resultado;
        }

        // Método para obtener un hospedaje por su ID
        public static Hospedaje ObtenerHospedajePorID(int id)
        {
            // Buscamos el hospedaje con el ID proporcionado
            return hospedajes.Find(h => h.Id == id);
        }
    }

    // Clase que representa un hospedaje
    public class Hospedaje
    {
        public int Id { get; set; }  // Identificador único del hospedaje
        public string Nombre { get; set; }
        public string Ubicacion { get; set; }
        public decimal PrecioPorNoche { get; set; }
        public int CapacidadPersonas { get; set; }
        public int Habitaciones { get; set; }
        public string Descripcion { get; set; }
    }
}
