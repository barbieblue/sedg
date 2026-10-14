using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class Clase : Actividad
    {
        public string TipoActividad { get; set; } = string.Empty;
        public Guid ProfesorId { get; set; }
        public Profesor Profesor { get; set; } = null!;
    }
}
