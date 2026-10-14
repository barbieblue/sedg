using System;
using System.Collections.Generic;
using System.Text;
using Dominio.Enums;

namespace Dominio.Entidades
{
    public class Descuento
    {
        public Guid Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public TipoDescuento Tipo { get; set; }
        public double Valor { get; set; }
        public DateOnly FechaInicio { get; set; } 
        public DateOnly FechaFin { get; set; } 
        public bool Activo { get; set; }
        public Guid CobroId { get; set; }
        public Cobro Cobro { get; set; } = null!;
    }
}
