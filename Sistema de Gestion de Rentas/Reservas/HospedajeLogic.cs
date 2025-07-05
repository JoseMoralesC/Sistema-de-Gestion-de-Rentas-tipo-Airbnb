using System;
using System.Collections.Generic;

namespace Sistema_de_Gestion_de_Rentas.Reservas
{
    public static class HospedajeLogic
    {
        // Estructura de los datos de los hospedajes
        public static Dictionary<string, Hospedaje> ObtenerHospedajePorProvincia(string provincia)
        {
            var hospedajesPorProvincia = new Dictionary<string, Hospedaje>();

            // Aquí agregamos los hospedajes de Cartago como ejemplo
            if (provincia == "Cartago")
            {
                hospedajesPorProvincia.Add("Hospedaje1", new Hospedaje
                {
                    Nombre = "Hospedaje A",
                    Ubicacion = "Cartago Centro",
                    PrecioPorNoche = 50,
                    CapacidadPersonas = 4,
                    Habitaciones = 2,
                    Descripcion = "Hermoso lugar en el centro de Cartago, ideal para parejas y familias pequeñas."
                });
                hospedajesPorProvincia.Add("Hospedaje2", new Hospedaje
                {
                    Nombre = "Hospedaje B",
                    Ubicacion = "Cartago Oeste",
                    PrecioPorNoche = 70,
                    CapacidadPersonas = 6,
                    Habitaciones = 3,
                    Descripcion = "Hospedaje cómodo y tranquilo, cerca de la naturaleza y lugares turísticos."
                });
                // Agregar más hospedajes si es necesario
            }

            // Se pueden agregar más provincias aquí, con más hospedajes...

            return hospedajesPorProvincia;
        }
    }

    public class Hospedaje
    {
        public string Nombre { get; set; }
        public string Ubicacion { get; set; }
        public decimal PrecioPorNoche { get; set; }
        public int CapacidadPersonas { get; set; }
        public int Habitaciones { get; set; }
        public string Descripcion { get; set; }
    }
}
