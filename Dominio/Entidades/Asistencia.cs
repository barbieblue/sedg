using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class Asistencia
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public string Estado { get; set; } = string.Empty;
        public Guid ActividadId { get; set; }
        public Actividad Actividad { get; set; } = null!;
        public Guid ClienteId { get; set; }
        public Cliente Cliente { get; set; } = null!;
    }
}
