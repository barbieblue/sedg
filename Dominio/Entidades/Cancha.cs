using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class Cancha
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Numero { get; set; } = 0;
        public EstadoCancha Estado { get; set; } = EstadoCancha.Disponible;
        //Foreign Key
        public Guid TipoCanchaId { get; set; }
        public TipoCancha TipoCancha { get; set; } = null!;

        // Relaciones de navegacion
        public ICollection<BloqueoCancha> Bloqueos { get; set; } = new List<BloqueoCancha>();
        public ICollection<Partido> Partidos { get; set; } = new List<Partido>();
        public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
        public ICollection<Actividad> Actividades { get; set; } = new List<Actividad>();
    }
}
