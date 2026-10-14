using System;
using System.Collections.Generic;
using System.Text;
using static Dominio.Entidades.EnumeracionesEstados;

namespace Dominio.Entidades
{
    public class TipoCancha
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nombre { get; set; } = string.Empty;
        public TipoSuperficie Superficie { get; set; }
        public int Capacidad { get; set; }
        public string Caracteristicas { get; set; } = string.Empty;

        // Relaciones 1:N con Cancha y Torneo
        public ICollection<Cancha> Canchas { get; set; } = new List<Cancha>();
        public ICollection<Torneo> Torneos { get; set; } = new List<Torneo>();
    }
}
