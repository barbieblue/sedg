using System;
using System.Collections.Generic;
using System.Text;
using static Dominio.Entidades.EnumeracionesEstados;

namespace Dominio.Entidades
{
    public class Partido
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Fecha { get; set; }
        public string Instancia { get; set; } = string.Empty; // Ej: "Octavos", "Cuartos", "Final"
        public string? Resultado { get; set; }                // Ej: "6-4, 4-6, 7-5"
        public EstadoPartido Estado { get; set; } = EstadoPartido.Programado;
        
        //relaciones Torneo
        public Guid TorneoId { get; set; }
        public Torneo Torneo { get; set; } = null!;
        //relaciones cancha
        public Guid CanchaId { get; set; }
        public Cancha Cancha { get; set; } = null!;

        // Relación N:N con competidores -- clientes
        public ICollection<Cliente> Competidores { get; set; } = new List<Cliente>();
    }
}
