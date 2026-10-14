using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicación.Interfaces
{
    public interface IInventarioQuery
    {
        Task<List<ItemInventario>> GetListItems();
        Task<ItemInventario?> GetItemById(Guid id);
        Task<ItemInventario?> GetItemByTipo(string tipo);
        Task<bool> HayStockDisponible(string tipo, int cantidadRequerida);
    }
}
