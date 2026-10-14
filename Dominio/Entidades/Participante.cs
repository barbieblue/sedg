using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class Participante
    {
        public Guid Id { get; set; }
        public string Dni { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public ICollection<Reserva>? Reservas { get; set; } 
    }
}
