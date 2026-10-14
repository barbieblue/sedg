using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class DetalleInventarioReserva
    {
        public Guid Id { get; set; }
        public int Cantidad { get; set; } 
        public bool Devuelto { get; set; }
        public ICollection<ItemInventario> ItemsInventario { get; set; } = new List<ItemInventario>();
    }
}
