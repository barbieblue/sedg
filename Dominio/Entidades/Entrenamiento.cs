using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class Entrenamiento : Actividad
    {
        public string TipoActividad { get; set; } = string.Empty;
        public Guid EntrenadorId { get; set; }
        public Entrenador Entrenador { get; set; } = null!;
    }
}
