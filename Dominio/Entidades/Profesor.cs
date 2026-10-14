using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class Profesor : Instructor
    {   
        public string Especialidad { get; set; } = string.Empty;
        // Clases que dicta el profesor
        public ICollection<Clase> Clases { get; set; } = new List<Clase>();
    }
}
