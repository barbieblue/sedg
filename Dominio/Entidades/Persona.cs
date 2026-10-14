using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public abstract class Persona
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Dni { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
