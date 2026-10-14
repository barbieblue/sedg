using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicación.Interfaces
{
    public interface IInventarioServices
    {
        Task CreateItem(ItemInventario item);
        Task UpdateItem(ItemInventario item);
        Task DeleteItem(Guid id);
        Task<List<ItemInventario>> GetAllItems();
        Task<ItemInventario?> GetItemById(Guid id);
        Task<bool> VerificarStock(string tipo, int cantidad);
        Task DescontarEquipamiento(Guid itemId, int cantidad);
        Task ReponerEquipamiento(Guid itemId, int cantidad);
    }
}
