using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class BloqueoCancha
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Motivo { get; set; } = string.Empty;

        // Apunta a la cancha que está siendo bloqueada
        public Guid CanchaId { get; set; }
        public Cancha Cancha { get; set; } = null!;
    }
}
