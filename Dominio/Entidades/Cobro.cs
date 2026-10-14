using Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class Cobro
    {
        public Guid Id { get; set; }
        public string TipoServicio { get; set; } = string.Empty;
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public double Importe { get; set; }
        public MedioPago MedioPago { get; set; } 
        public string QrGenerado { get; set; } = string.Empty;
        public EstadoCobro Estado { get; set; } 
        public string TipoCobro { get; set; } = string.Empty;

        public Guid ClienteId { get; set; }
        public Cliente Cliente { get; set; } = null!;

        public Guid? ActividadId { get; set; }
        public Actividad? Actividad { get; set; }
        public Guid? ReservaId { get; set; }
        public Reserva? Reserva { get; set; }
        public Recibo? Recibo { get; set; }
        public ICollection<Descuento>? Descuentos { get; set; }
    }
}
