using System;

namespace Sistema_de_Gestion_de_Rentas.Reservas
{
    public class Reserva
    {
        public int HuespedId { get; set; }
        public int HospedajeId { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public int CantidadPersonas { get; set; }
        public int CantidadNoches { get; set; }

        public int ProvinciaId { get; set; } 

        public Reserva() { }

        public Reserva(int huespedId, int hospedajeId, DateTime fechaEntrada, DateTime fechaSalida,
                       int cantidadPersonas, int cantidadNoches)
        {
            if (fechaSalida < fechaEntrada)
                throw new ArgumentException("La fecha de salida no puede ser anterior a la fecha de entrada.");
            if (cantidadPersonas <= 0)
                throw new ArgumentException("La cantidad de personas debe ser mayor a cero.");
            if (cantidadNoches <= 0)
                throw new ArgumentException("La cantidad de noches debe ser mayor a cero.");

            HuespedId = huespedId;
            HospedajeId = hospedajeId;
            FechaEntrada = fechaEntrada;
            FechaSalida = fechaSalida;
            CantidadPersonas = cantidadPersonas;
            CantidadNoches = cantidadNoches;
        }

        public double CalcularTotal(double precioPorNoche)
        {
            if (precioPorNoche <= 0)
                throw new ArgumentException("El precio por noche debe ser mayor a cero.");
            return CantidadNoches * precioPorNoche;
        }

        public string DescripcionReserva()
        {
            return $"Reserva para {CantidadPersonas} personas, desde {FechaEntrada.ToShortDateString()} hasta {FechaSalida.ToShortDateString()}.";
        }
    }

}
