using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class Entrenador : Instructor
    {
        public string EspecialidadDeportiva { get; set; } = string.Empty;
        // Entrenamientos que dirige el entrenador
        public ICollection<Entrenamiento> Entrenamientos { get; set; } = new List<Entrenamiento>();
    }
}
