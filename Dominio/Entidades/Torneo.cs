using System;
using System.Collections.Generic;
using System.Text;
using static Dominio.Entidades.EnumeracionesEstados;

namespace Dominio.Entidades
{
    public class Torneo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nombre { get; set; } = string.Empty;
        public string Disciplina { get; set; } = "Tenis";
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public ModalidadTorneo Modalidad { get; set; }
        public CategoriaTorneo Categoria { get; set; }
        public int CantidadMinimaParticipantes { get; set; }
        public int CantidadMaximaParticipantes { get; set; }
        public EstadoTorneo Estado { get; set; } = EstadoTorneo.Planificado;

        // FK, tipo de cancha solicitado para el torneo y Relacion con inscripcion
        public Guid TipoCanchaId { get; set; }
        public TipoCancha TipoCancha { get; set; } = null!;
        public ICollection<InscripcionTorneo> Inscripciones { get; set; } = new List<InscripcionTorneo>();
        public ICollection<Partido> Partidos { get; set; } = new List<Partido>();
    }
}
