using System;
using System.Collections.Generic;
using System.Text;
using Dominio.Enums;

namespace Dominio.Entidades
{
    public class Recibo
    {
        public Guid Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public string Concepto { get; set; } = string.Empty;
        public double Importe { get; set; } 
        public MedioPago MedioPago { get; set; }

        public Guid CobroId { get; set; }
        public Cobro Cobro { get; set; } = null!;
    }
}
