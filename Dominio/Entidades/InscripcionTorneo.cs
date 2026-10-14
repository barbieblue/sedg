using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class InscripcionTorneo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime FechaInscripcion { get; set; } = DateTime.UtcNow;

        // Relación con Torneo
        public Guid TorneoId { get; set; }
        public Torneo Torneo { get; set; } = null!;

        // Cliente principal inscripto y participantes
        public Guid ClienteId { get; set; }
        public Cliente Cliente { get; set; } = null!;
        public ICollection<Cliente> Participantes { get; set; } = new List<Cliente>();
    }
}
