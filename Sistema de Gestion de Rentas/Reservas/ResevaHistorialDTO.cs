using System;

namespace Sistema_de_Gestion_de_Rentas.Reservas
{
    public class ReservaHistorialDTO
    {
        public int Id { get; set; }
        public int HuespedId { get; set; }
        public int HospedajeId { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public int CantidadPersonas { get; set; }
        public int CantidadNoches { get; set; }
        public bool Estado { get; set; } 
    }
}
