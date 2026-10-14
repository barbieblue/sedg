using System;
using System.Collections.Generic;
using System.Text;
using Dominio.Enums;

namespace Dominio.Entidades
{
    public class Reserva
    {
        public Guid Id { get; set; }
        public DateOnly Fecha { get; set; } 
        public TimeOnly HoraInicio { get; set; } = TimeOnly.MinValue;
        public TimeOnly HoraFin { get; set; } = TimeOnly.MinValue;
        public EstadoReserva Estado { get; set; } 
        public double MontoTotal { get; set; } 
        public bool AnticipoPagado { get; set; } 

        public Guid ClienteId { get; set; }
        public Cliente Cliente { get; set; } = null!;

        public Guid CanchaId { get; set; }
        public Cancha Cancha { get; set; } = null!;

        public ICollection<Participante> Participantes { get; set; } = new List<Participante>();
        public ICollection<DetalleInventarioReserva> DetalleInventario { get; set; } = new List<DetalleInventarioReserva>();
        public ICollection<Cobro> Cobros { get; set; } = new List<Cobro>();
    }
}
