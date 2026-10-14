using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class Cliente : Persona
    {
        public string? Direccion { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        // Relación: Un cliente tiene múltiples reservas
        public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
        public ICollection<Clase> Clases { get; set; } = new List<Clase>();
        public ICollection<Asistencia> Asistencias { get; set; } = new List<Asistencia>();
    }
}
