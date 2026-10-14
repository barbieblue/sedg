using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
	public class Actividad
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public DateTime Fecha { get; set; } = DateTime.MinValue;
		public TimeOnly HoraInicio { get; set; } = TimeOnly.MinValue;
		public TimeOnly HoraFin { get; set; } = TimeOnly.MinValue;
		public int CapacidadMaxima { get; set; } = 30;
		public EstadoActividad Estado { get; set; } = EstadoActividad.Disponible;

		// Relaciones
		public ICollection<Cancha> Canchas { get; set; } = new List<Cancha>();
		public ICollection<Asistencia> Asistencias { get; set; } = new List<Asistencia>();
		public ICollection<Cobro> Cobros { get; set; } = new List<Cobro>();
		public ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
	}
}
