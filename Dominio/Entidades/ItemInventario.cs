using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class ItemInventario
    {
        public Guid Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public int StockTotal { get; set; } 
        public int StockDisponible { get; set; }
        public ICollection<DetalleInventarioReserva> InventarioReservas { get; set; } = new List<DetalleInventarioReserva>();
    }
}
