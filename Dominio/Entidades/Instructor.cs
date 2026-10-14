using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class Instructor: Persona
    {
        public string TituloHabilitante { get; set; } = string.Empty;
        public DateTime FechaVerificacion { get; set; } = DateTime.MinValue;
    }
}
